using System.Collections.Generic;
using SimpleJSON;
using ROSBridgeLib.std_msgs;
using UnityEngine;

/* Joint trajctory message under trajectorymsgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace trajectory_msgs
    {
        public class JointTrajectoryMsg : ROSBridgeMsg
        {
            private HeaderMsg _header;
            private string[] _joint_names;
            private List<JointTrajectoryPointMsg> _points = new List<JointTrajectoryPointMsg>();

            public JointTrajectoryMsg(JSONNode msg)
            {
                if (msg == null)
                {
                    Debug.Log("Msg is null");
                }
                _header = new HeaderMsg(msg["header"]);
                _joint_names = new string[msg["joint_names"].Count];
              
                for (int i = 0; i < msg["joints_name"].Count; i++)
                {
                    _joint_names[i] = (msg["joint_names"][i]);
                }

                for (int i = 0; i < msg["points"].Count; i++)
                {
                    _points.Add(new JointTrajectoryPointMsg(msg["points"][i]));
                }
            }

            public JointTrajectoryMsg(HeaderMsg header, string[] joints_name, List<JointTrajectoryPointMsg> points)
            {
                _header = header;
                _joint_names = joints_name;
                _points = points;
            }

            public static string getMessageType()
            {
                return "trajectory_msgs/JointTrajectory";
            }

            public string[] GetNames()
            {
                return _joint_names;
            }

            public JointTrajectoryPointMsg GetPoint(int idx = 0) //get joint trajectory point at idx
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
                for(int i = 0; i< _points.Count; i++)
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
