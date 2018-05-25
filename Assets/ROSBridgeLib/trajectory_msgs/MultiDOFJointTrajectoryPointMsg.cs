using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.geometry_msgs;

/* Multi DOF Joint trajctory Point message under trajectorymsgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace trajectory_msgs
    {
        public class MultiDOFJointTrajectoryPointMsg : ROSBridgeMsg
        {
            private TransformMsg[] _transforms;
            private TwistMsg[] _velocities;
            private TwistMsg[] _accelerations;
            DurationMsg _time_from_start;

            public MultiDOFJointTrajectoryPointMsg(JSONNode msg)
            {
                _transforms = new TransformMsg[msg["transforms"].Count];
                for (int i = 0; i < _transforms.Length; i++)
                {
                    _transforms[i] = new TransformMsg(msg["positions"][i]);
                }
                _velocities = new TwistMsg[msg["velocities"].Count];
                for (int i = 0; i < _velocities.Length; i++)
                {
                    _velocities[i] = new TwistMsg(msg["velocities"][i]);
                }
                _accelerations = new TwistMsg[msg["accelerations"].Count];
                for (int i = 0; i < _accelerations.Length; i++)
                {
                    _accelerations[i] = new TwistMsg(msg["accelerations"][i]);
                }
          
                _time_from_start = new DurationMsg(msg["time_from_start"]);

            }

            public MultiDOFJointTrajectoryPointMsg(TransformMsg[] transforms, TwistMsg[] velocities,
                TwistMsg[] accelerations, DurationMsg time_from_start)
            {
                _transforms = transforms;
                _velocities = velocities;
                _accelerations = accelerations;
                _time_from_start = time_from_start;
            }

            public static string getMessageType()
            {
                return "trajectory_msgs/MultiDOFJointTrajectoryPoint";
            }

            public TransformMsg[] GetTransforms()
            {
                return _transforms;
            }

            public TwistMsg[] GetVelocities()
            {
                return _velocities;
            }

            public TwistMsg[] GetAccelerations()
            {
                return _accelerations;
            }

            public DurationMsg GetDuration()
            {
                return _time_from_start;
            }

            public override string ToString()
            {

                return "MultiDOFJointTrajectoryPoint [transforms=" + _transforms.ToString() +", velocities= " + _velocities.ToString()
                    + ",  accelerations=" + _accelerations.ToString() + ",  time_frome_start=" + _time_from_start.ToString() + "]";
            }

            public override string ToYAMLString()
            {

                //converting transform array to yaml string
                string namearray = "[";
                for (int i = 0; i < _transforms.Length; i++)
                {
                    namearray = namearray + _transforms[i].ToYAMLString();
                    if (_transforms.Length - i >= 1 && i < _transforms.Length - 1)
                        namearray += ",";
                }
                namearray += "]";

                //converting velocities array to yaml string
                string velarray = "[";
                for (int i = 0; i < _velocities.Length; i++)
                {
                    velarray = velarray + _velocities[i].ToYAMLString();
                    if (_velocities.Length - i >= 1 && i < _velocities.Length - 1)
                        velarray += ",";
                }
                velarray += "]";

                //converting accelerations array to yaml string
                string accelarray = "[";
                for (int i = 0; i < _accelerations.Length; i++)
                {
                    accelarray = accelarray + _accelerations[i].ToYAMLString();
                    if (_accelerations.Length - i >= 1 && i < _accelerations.Length - 1)
                        accelarray += ",";
                }
                accelarray += "]";

                return "{\"transforms\" : " +namearray + ", \"velocities\" : " + velarray
                    + ", \"accelerations\" : " + accelarray + ", \"time_from_start\" : " + _time_from_start.ToYAMLString() + "}";
            }

            private string floattoarray(float[] input) //create the array
            {
                string floatarray = "[";
                for (int i = 0; i < input.Length; i++)
                {
                    floatarray = floatarray + input[i];
                    if (input.Length - i >= 1 && i < input.Length - 1)
                        floatarray += ",";
                }
                floatarray += "]";
                return floatarray;
            }
        }
    }
}
