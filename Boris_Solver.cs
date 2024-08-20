public class Vector3 {
    public double x, y, z;

    public Vector3(double x, double y, double z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    static public Vector3 Zero {
        get {
            return new Vector3(0, 0, 0);
        }
    }

    static public Vector3 Up {
        get {
            return new Vector3(0, 0, 1);
        }
    }

    public static Vector3 operator +(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }
    public static Vector3 operator -(Vector3 v1, Vector3 v2) {
        return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }
    public static Vector3 operator -(Vector3 v1) {
        return new Vector3(-v1.x, -v1.y, -v1.z);
    }
    public static Vector3 operator *(double a, Vector3 v) {
        return new Vector3(v.x * a, v.y * a, v.z * a);
    }
    public static Vector3 operator *(Vector3 v, double a) {
        return new Vector3(v.x * a, v.y * a, v.z * a);
    }
    public static Vector3 operator /(Vector3 v, double a) {
        return new Vector3(v.x / a, v.y / a, v.z / a);
    }

    static public Vector3 Cross(Vector3 former, Vector3 latter) {
        double x = former.y * latter.z - former.z * latter.y;
        double y = -(former.x * latter.z - former.z * latter.x);
        double z = former.x * latter.y - former.y * latter.x;
        return new Vector3(x, y, z);
    }

    static public double Dot(Vector3 former, Vector3 latter) {
        double x = former.x * latter.x;
        double y = former.y * latter.y;
        double z = former.z * latter.z;
        return x + y + z;
    }
    /// <summary>
    /// return zero vector for zero vector
    /// </summary>
    /// <returns></returns>
    public Vector3 Normalized() {
        double l = Math.Sqrt(x * x + y * y + z * z);
        if (l <= 0) return new Vector3(0, 0, 0);
        double x1 = x / l;
        double y1 = y / l;
        double z1 = z / l;
        return new Vector3(x1, y1, z1);
    }

    public double Lorentz_Factor_from_U(double c = 299792458) {
        double u_square = x * x + y * y + z * z;
        double beta1 = u_square / (c * c);
        return Math.Sqrt(1 + beta1);
    }
    public double Lorentz_Factor_from_V(double c = 299792458) {
        double v_square = x * x + y * y + z * z;
        double beta1 = v_square / (c * c);
        if (1 - beta1 <= 0) throw new Exception("faster than light");
        return 1 / Math.Sqrt(1 - beta1);
    }

    public double Magnitude() {
        return Math.Sqrt(x * x + y * y + z * z);
    }

    public Vector3 Copy() {
        return new Vector3(x, y, z);
    }

    public string ToStr() {
        string text = $"({x}, {y}, {z})";
        return text;
    }
}
abstract public class Field {
    protected List<Field_Type> types;

    abstract public class Field_Type {
        abstract public Vector3 Get_Vector(Vector3 pos);
    }

    public Vector3 Get_Vector(Vector3 pos) {
        int count = types.Count;
        Vector3 vector_sum = Vector3.Zero;
        
        for (int i = 0; i < count; i++) {
            vector_sum += types[i].Get_Vector(pos);
        }
        return vector_sum;
    }
}

public class E_Field : Field {

    public class E_Type : Field_Type {
        public E_Field_Type type;
        private Vector3 origin;
        private Vector3 direction;
        private double magnitude;
        public enum E_Field_Type {
            point,
            dipole,
            uniform,
            line,
        }

        public override Vector3 Get_Vector(Vector3 pos) {
            if (type == E_Field_Type.point) {
                return (magnitude / Math.Pow((pos - origin).Magnitude(), 2)) * (pos - origin).Normalized();
            }
            else if (type == E_Field_Type.uniform) {
                return magnitude * direction;
            }
            else if (type == E_Field_Type.line) {
                double t = (Vector3.Dot(pos, direction) - Vector3.Dot(direction, origin)) / Vector3.Dot(direction, direction);
                Vector3 s = origin + t * direction; //s is shortest point to pos on line

                double distance = (pos - s).Magnitude();
                return magnitude * (pos - s).Normalized() / distance;
            }
            else if (type == E_Field_Type.dipole) {
                double l = (pos - origin).Magnitude();
                Vector3 d = direction.Normalized();
                Vector3 p = (pos - origin).Normalized();
                return (Vector3.Dot(d, p) * p - d) * magnitude / Math.Pow(l, 3);
            }
            else return Vector3.Zero;
        }

        private E_Type(E_Field_Type type, Vector3 origin, Vector3 direction, double magnitude = 1) {
            this.type = type;
            this.origin = origin;
            this.direction = direction;
            this.magnitude = magnitude;
        }
        public static E_Type Point_Field(Vector3 origin, double magnitude = 1) {
            return new E_Type(E_Field_Type.point, origin, Vector3.Zero, magnitude); ;
        }
        public static E_Type Uniform_Field(Vector3 direction, double magnitude = 1) {
            return new E_Type(E_Field_Type.uniform, Vector3.Zero, direction, magnitude);
        }
        public static E_Type Line_Field(Vector3 origin, Vector3 current_direction, double magnitude = 1) {
            return new E_Type(E_Field_Type.line, origin, current_direction, magnitude);
        }
        /// <summary>
        /// positive is north
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="axis_direction"></param>
        /// <param name="magnitude">strength at equator distance 1</param>
        /// <returns></returns>
        public static E_Type Dipole_Field(Vector3 origin, Vector3 axis_direction, double magnitude = 1) {
            return new E_Type(E_Field_Type.dipole, origin, axis_direction, magnitude);
        }

    }
    /// <summary>
    /// put null if there is no e field
    /// </summary>
    /// <param name="types"></param>
    public E_Field(List<E_Type> types = null) {
        if (types == null) this.types = new List<Field_Type>();
        else this.types = types.Select(x=>(Field_Type)x).ToList();
    }
}

public class B_Field : Field {
    public class B_Type : Field_Type {
        public B_Field_Type type;
        private Vector3 origin;
        private Vector3 direction;
        private double magnitude;
        public enum B_Field_Type {
            point,
            dipole,
            uniform,
            line,
        }

        public override Vector3 Get_Vector(Vector3 pos) {
            if (type == B_Field_Type.point) {
                return (magnitude / Math.Pow((pos - origin).Magnitude(), 2)) * (pos - origin).Normalized();
            }
            else if (type == B_Field_Type.uniform) {
                return magnitude * direction;
            }
            else if (type == B_Field_Type.line) {
                double t = (Vector3.Dot(pos, direction) - Vector3.Dot(direction, origin)) / Vector3.Dot(direction, direction);
                Vector3 s = origin + t * direction; //s is shortest point to pos on line

                double distance = (pos - s).Magnitude();
                return magnitude * Vector3.Cross(direction, pos - s).Normalized() / distance;
            }
            else if (type == B_Field_Type.dipole) {
                double l = (pos - origin).Magnitude();
                Vector3 d = direction.Normalized();
                Vector3 p = (pos - origin).Normalized();
                return (Vector3.Dot(d, p) * p - d) * magnitude / Math.Pow(l, 3);
            }
            else return Vector3.Zero;
        }

        private B_Type(B_Field_Type type, Vector3 origin, Vector3 direction, double magnitude = 1) {
            this.type = type;
            this.origin = origin;
            this.direction = direction;
            this.magnitude = magnitude;
        }

        /// <summary>
        /// make monopole
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="magnitude">negative for -</param>
        /// <returns></returns>
        public static B_Type Point_Field(Vector3 origin, double magnitude = 1) {
            return new B_Type(B_Field_Type.point, origin, Vector3.Zero, magnitude); ;
        }
        public static B_Type Uniform_Field(Vector3 direction, double magnitude = 1) {
            return new B_Type(B_Field_Type.uniform, Vector3.Zero, direction, magnitude);
        }
        public static B_Type Line_Field(Vector3 origin, Vector3 current_direction, double magnitude = 1) {
            return new B_Type(B_Field_Type.line, origin, current_direction, magnitude);
        }
        public static B_Type Dipole_Field(Vector3 origin, Vector3 axis_direction, double magnitude = 1) {
            return new B_Type(B_Field_Type.dipole, origin, axis_direction, magnitude);
        }

    }
    /// <summary>
    /// put null if there is no b field
    /// </summary>
    /// <param name="types"></param>
    public B_Field(List<B_Type> types = null) {
        if (types == null) this.types = new List<Field_Type>();
        else this.types = types.Select(x => (Field_Type)x).ToList();
    }
    
}

public class Boris_Solver {
    double q, m, t_dif;
    Vector3 U_before, U_after, X_before, X_after;
    Field E_field, B_field;
    public decimal x_step, u_step;
    public Boris_Solver(double charge, double mass, double time_dif, E_Field E_field, B_Field B_field, Vector3 init_vel, Vector3 init_pos) {
        q = charge;
        m = mass;
        t_dif = time_dif;
        this.E_field = E_field;
        this.B_field = B_field;
        U_after = init_vel * init_vel.Lorentz_Factor_from_V();
        X_after = init_pos + init_vel * (time_dif/2);
        x_step = 0.5m;
        u_step = 0.0m;
    }

    public Vector3 Get_Next_Position() {
        Update_U();
        Update_X();
        return X_after;
    }

    public Vector3 Get_E_field(Vector3 pos) {
        return E_field.Get_Vector(pos).Copy();
    }
    public Vector3 Get_B_field(Vector3 pos) {
        return B_field.Get_Vector(pos).Copy();
    }

    private void Update_U() {
        U_before = U_after;
        Vector3 epsilon = (q / (2 * m)) * E_field.Get_Vector(X_after);
        Vector3 U_minus = U_before + epsilon * t_dif;

        double theta = q * t_dif * B_field.Get_Vector(X_after).Magnitude() / (m * U_minus.Lorentz_Factor_from_U());

        Vector3 t = (theta / 2) * B_field.Get_Vector(X_after).Normalized();
        Vector3 U_prime = U_minus + Vector3.Cross(U_minus, t);
        Vector3 U_plus = U_minus + (2 / (1 + Math.Pow(t.Magnitude(), 2))) * Vector3.Cross(U_prime, t);

        U_after = U_plus + epsilon * t_dif;
        u_step += 1.0m;
    }

    private void Update_X() {
        X_before = X_after;
        X_after = U_after * t_dif / U_after.Lorentz_Factor_from_U() + X_before;
        x_step += 1.0m;
    }
    

}
