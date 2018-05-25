///*  Pointcloud Msg
// *  added to ROSBridgeLib by Cole Shing, 2017
// *  std_msgs/Header header
// *  geometry_msgs/Point32[] points
// *  sensor_msgs/ChannelFloat32[] channels
// */

//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using SimpleJSON;
//using ROSBridgeLib.std_msgs;
//using ROSBridgeLib.geometry_msgs; //for point32

//namespace ROSBridgeLib {
//		namespace sensor_msgs {
//				public class PointCloudMsg : ROSBridgeMsg
//                {
//                    private HeaderMsg _header;
//                    private Point32Msg[] _points;
//                    private float[] channels;

//                    public PointCloudMsg(JSONNode msg){
//                        _header = new HeaderMsg(msg["header"]);
//                        _points = new Point32Msg[msg["points"].Count];
//                        for (int i = 0; i < _points.Length; i++)
//                        {
                            
                    
//                        }   
//                _rate_lim = new float[msg["rate_limits"].Count];
//                for (int i = 0; i < _rate_lim.Length; i++)
//                {
//                    _rate_lim[i] = float.Parse(msg["rate_limits"][i]);
//                }
//            }

//            public PointCloudMsg(HeaderMsg header, Point32Msg[] points, float[] channels)
//            {
//                _header = header;
//                _points = points;
//                _channels = channels;
//            }

//            public static string GetMessageType()
//            {
//                return "wam_common/RTJointPos";
//            }

//            public float[] GetJoints()
//            {
//                return _joints;
//            }

//            public float[] GetRate_Limits()
//            {
//                return _rate_lim;
//            }

//            public override string ToString()
//            {
//                //converting the joints array into string
//                string jointarray = "[";
//                for (int i = 0; i < _joints.Length; i++)
//                {
//                    jointarray = jointarray + _joints[i];
//                    if (_joints.Length - i >= 1 && i < _joints.Length - 1)
//                        jointarray += ",";
//                }
//                jointarray += "]";

//                //converting the rate_limits array into string
//                string ratearray = "[";
//                for (int i = 0; i < _rate_lim.Length; i++)
//                {
//                    ratearray = ratearray + _rate_lim[i];
//                    if (_rate_lim.Length - i >= 1 && i < _rate_lim.Length - 1)
//                        ratearray += ",";
//                }
//                ratearray += "]";

//                return "RTJointPos [joints=" + jointarray + ", rate_limits= " + ratearray + "]";
//            }

//            public override string ToYAMLString()
//            {
//                //converting the joints array into YAMLstring
//                string jointarray = "[";
//                for (int i = 0; i < _joints.Length; i++)
//                {
//                    jointarray = jointarray + _joints[i];
//                    if (_joints.Length - i >= 1 && i < _joints.Length - 1)
//                        jointarray += ",";
//                }
//                jointarray += "]";

//                //converting the rate_limits array into YAMLstring
//                string ratearray = "[";
//                for (int i = 0; i < _rate_lim.Length; i++)
//                {
//                    ratearray = ratearray + _rate_lim[i];
//                    if (_rate_lim.Length - i >= 1 && i < _rate_lim.Length - 1)
//                        ratearray += ",";
//                }
//                ratearray += "]";

//                return "{\"joints\" : " + jointarray + ", \"rate_limits\" : " + ratearray + "}";
//            }
//        }
//    }
//}

