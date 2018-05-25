using SimpleJSON;
using ROSBridgeLib.std_msgs;

/* Joint trajctory message under trajectorymsgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace trajectory_msgs
    {
        public class JointTrajectoryPointMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private double[] _positions;
            private double[] _velocities;
            private double[] _accelerations;
            private double[] _effort;
            DurationMsg _time_from_start;

            public JointTrajectoryPointMsg(JSONNode msg)
            {
                _positions = new double[msg["positions"].Count];
                for (int i = 0; i < _positions.Length; i++)
                {
                    _positions[i] = double.Parse(msg["positions"][i]);
                }
                _velocities = new double[msg["velocities"].Count];
                for (int i = 0; i < _velocities.Length; i++)
                {
                    _velocities[i] = double.Parse(msg["velocities"][i]);
                }
                _accelerations = new double[msg["accelerations"].Count];
                for (int i = 0; i < _accelerations.Length; i++)
                {
                    _accelerations[i] = double.Parse(msg["accelerations"][i]);
                }
                _effort = new double[msg["effort"].Count];
                for (int i = 0; i < _effort.Length; i++)
                {
                    _effort[i] = double.Parse(msg["effort"][i]);
                }

                _time_from_start = new DurationMsg(msg["time_from_start"]);

            }

            public JointTrajectoryPointMsg(double[] positions, double[] velocities,
                double[] accelerations, double[] effort, DurationMsg time_from_start)
            {
                _positions = positions;
                _velocities = velocities;
                _accelerations = accelerations;
                _effort = effort;
                _time_from_start = time_from_start;
            }

            public static string getMessageType() {
                return "trajectory_msgs/JointTrajectoryPoint";
            }

            public double[] GetPositions() {
                return _positions;
            }

            public double[] GetVelocities() {
                return _velocities;
            }

            public double[] GetAccelerations() {
                return _accelerations;
            }

            public double[] GetEffort() { 
                return _effort;
            }

            public DurationMsg GetDuration() { 
                return _time_from_start;
            }

            public override string ToString()
            {
                //converting the position array into string
                string posarray = convert.doubletoarray(_positions);

                //converting the velocity array into string
                string velarray = convert.doubletoarray(_velocities);

                //converting the acceleration array into string
                string accelarray = convert.doubletoarray(_accelerations);

                //converting the effort array into string
                string effarray = convert.doubletoarray(_effort);

                return "JointTrajectoryPoint [positions=" + posarray + ", velocities= " + velarray 
                    + ",  accelerations=" + accelarray + ",  effort=" + effarray 
                    + ",  time_frome_start=" + _time_from_start.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                //converting the position array into string
                string posarray = convert.doubletoarray(_positions);

                //converting the velocity array into string
                string velarray = convert.doubletoarray(_velocities);

                //converting the acceleration array into string
                string accelarray = convert.doubletoarray(_accelerations);

                //converting the effort array into string
                string effarray = convert.doubletoarray(_effort);
                
                return "{\"positions\" : " + posarray + ", \"velocities\" : " + velarray 
                    + ", \"accelerations\" : " + accelarray + ", \"effort\" : " + velarray
                    + ", \"time_from_start\" : " + _time_from_start.ToYAMLString() + "}";
            }
        }
    }
}
