using SimpleJSON;
using ROSBridgeLib.std_msgs;
using System.Collections.Generic;

/* Multi DOF Joint trajctory message under trajectorymsgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace trajectory_msgs
    {
        public class MultiDOFJointTrajectoryMsg : ROSBridgeMsg
        {
            private HeaderMsg _header;
            private string[] _joint_names;
            private List<MultiDOFJointTrajectoryPointMsg> _points = new List<MultiDOFJointTrajectoryPointMsg>();

            public MultiDOFJointTrajectoryMsg(JSONNode msg)
            {
                _header = new HeaderMsg(msg["header"]);
                _joint_names = new string[msg["joints_name"].Count];
                for (int i = 0; i < _joint_names.Length; i++)
                {
                    _joint_names[i] = (msg["joint_names"][i]);
                }

                for (int i = 0; i < msg["points"].Count; i++)
                {
                    _points.Add(new MultiDOFJointTrajectoryPointMsg(msg["points"][i]));
                }
            }

            public MultiDOFJointTrajectoryMsg(HeaderMsg header, string[] joints_name, List<MultiDOFJointTrajectoryPointMsg> points)
            {
                _header = header;
                _joint_names = joints_name;
                _points = points;
            }

            public static string getMessageType()
            {
                return "trajectory_msgs/MultiDOFJointTrajectory";
            }

            public string[] GetNames()
            {
                return _joint_names;
            }

            public List<MultiDOFJointTrajectoryPointMsg> GetPoints()
            {
                return _points;
            }

            public MultiDOFJointTrajectoryPointMsg GetPoint(int idx = 0) //get joint trajectory point at idx
            {
                if (idx < _points.Count)
                    return _points[idx];
                else
                    return null;
            }

            public override string ToString()
            {
                //converting the name array into string
                string namearray = "[";
                for (int i = 0; i < _joint_names.Length; i++)
                {
                    namearray = namearray + _joint_names[i];
                    if (_joint_names.Length - i >= 1 && i < _joint_names.Length - 1)
                        namearray += ",";
                }
                namearray += "]";

                string pointarray = "[";
                for (int i = 0; i < _points.Count; i++)
                {
                    pointarray = pointarray + _points[i].ToString();
                    if (_points.Count - i >= 1 && i < _points.Count - 1)
                        pointarray = ",";
                }
                pointarray += "]";

                return "JointTrajectory [header=" + _header.ToString() + ", joint_names= " + namearray
                    + ",  points=" + pointarray + "]";

            }

            public override string ToYAMLString()
            {
                //converting the name array into YAMLstring
                string namearray = "{";
                for (int i = 0; i < _joint_names.Length; i++)
                {
                    namearray = namearray + _joint_names[i];
                    if (_joint_names.Length - i >= 1 && i < _joint_names.Length - 1)
                        namearray += ",";
                }
                namearray += "}";

                //converting points array to yaml string
                string pointarray = "{";
                for (int i = 0; i < _points.Count; i++)
                {
                    pointarray = pointarray + _points[i].ToYAMLString();
                    if (_points.Count - i >= 1 && i < _points.Count - 1)
                        pointarray += ",";
                }
                pointarray += "}";

                return "{\"header\" : " + _header.ToYAMLString() + ", \"joint_names\" : " + namearray
                     + ", \"points\" : " + pointarray + "}";
            }
        }
    }
}
