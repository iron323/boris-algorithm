class Boris_Solver {
    public class Vector3 {
        public double x, y, z;

        public Vector3(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator - (Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3 operator - (Vector3 v1) {
            return new Vector3(-v1.x, -v1.y, -v1.z);
        }
        public static Vector3 operator * (double a, Vector3 v) {
            return new Vector3(v.x * a, v.y * a, v.z * a);
        }
        public static Vector3 operator * (Vector3 v, double a) {
            return new Vector3(v.x * a, v.y * a, v.z * a);
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

        public Vector3 Normalized() {
            double l = Math.Sqrt(x * x + y * y + z * z);
            if (l <= 0) throw new Exception("zero vector cannot be normalized");
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

        public Vector3 Copy() {
            return new Vector3(x, y, z);
        }
    }

    public class Field {
        Vector3 field;
        public Field(Vector3 v) {
            field = v;
        }
        public Vector3 Get_Vector(Vector3 pos) {
            return field.Copy();
        }
    }

    double q, m, t_diff;
    Vector3 U_before, U_after, X_before, X_after;
    Field E_field, B_field;
    decimal x_step, u_step;
    public Boris_Solver(double charge, double mass, double time_diff, Field E_field, Field B_field, Vector3 init_vel, Vector3 init_pos) {
        q = charge;
        m = mass;
        t_diff = time_diff;
        this.E_field = E_field;
        this.B_field = B_field;
        U_before = init_vel;
        X_before = init_pos;
        x_step = 0.0m;
        u_step = 0.0m;
    }

    public Vector3 Get_Next_Position() {

    }

    private Vector3 Get_U_from_V() {

    }
    


}
