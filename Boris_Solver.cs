class Boris_Solver {
    public class Vector3 {
        public double x, y, z;

        public Vector3(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        static public Vector3 Cross(Vector3 former, Vector3 latter) {
            double x = former.y * latter.z - former.z * latter.y;
            double y = -(former.x * latter.z - former.z * latter.x);
            double z = former.x * latter.y - former.y * latter.x;
            return new Vector3(x, y, z);
        }

        static public Vector3 Dot(Vector3 former, Vector3 latter) {
            double x = former.x * latter.x;
            double y = former.y * latter.y;
            double z = former.z * latter.z;
            return new Vector3(x, y, z);
        }

        static public double Get_Lorentz_Factor(Vector3 velocity, double speed_of_light = 299792458) {
            double v_square = velocity.x * velocity.x + velocity.y * velocity.y + velocity.z * velocity.z;
            double beta = v_square / (speed_of_light * speed_of_light);
            return 1 / Math.Sqrt(1 - beta);
        }

    }

    double q, m, t_diff;
    Vector3 E_field, B_field, V, U;
    decimal step;
    public Boris_Solver(double charge, double mass, double time_diff, Vector3 E_field, Vector3 B_field, Vector3 Initial_Velocity) {
        q = charge;
        m = mass;
        t_diff = time_diff;
        this.E_field = E_field;
        this.B_field = B_field;
        V = Initial_Velocity;
        step = 0.0m;
    }

    public Vector3 Get_Next_Position() {

    }

    private Vector3 Get_U_from_V() {

    }
    


}
