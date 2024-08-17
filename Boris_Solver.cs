public class Vector3 {
    public double x, y, z;

    public Vector3(double x, double y, double z) {
        this.x = x;
        this.y = y;
        this.z = z;
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

public class Boris_Solver {


    public class Field {
        Vector3 field;
        public Field(Vector3 v) {
            field = v;
        }
        public Vector3 Get_Vector(Vector3 pos) {
            return field.Copy();
        }
    }

    double q, m, t_dif;
    Vector3 U_before, U_after, X_before, X_after;
    Field E_field, B_field;
    public decimal x_step, u_step;
    public Boris_Solver(double charge, double mass, double time_dif, Field E_field, Field B_field, Vector3 init_vel, Vector3 init_pos) {
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
