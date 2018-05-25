using SimpleJSON;
using ROSBridgeLib.std_msgs;

/*  WrenchStamped Msg   
 *  added to ROSBridgeLib and modified by Cole Shing, 2018
 *  std_msgs/Header header
 *  Geometry_msgs/Wrench wrench
 *      geometry_msgs/Vector3 force
 *      geometry_msgs/Vector3 torque
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class WrenchStampedMsg : ROSBridgeMsg
        {
            public HeaderMsg _header;
            public WrenchMsg _wrench;

            public WrenchStampedMsg(JSONNode msg)
            {
                _header = new HeaderMsg(msg["header"]);
                _wrench = new WrenchMsg(msg["wrench"]);
            }

            public WrenchStampedMsg(HeaderMsg header, WrenchMsg wrench)
            {
                _header = header;
                _wrench = wrench;
            }
            public static string GetMessageType()
            {
                return "geometry_msgs/WrenchStamped";
            }

            public HeaderMsg GetHeader()
            {
                return _header;
            }

            public WrenchMsg GetWrench()
            {
                return _wrench;
            }

            public Vector3Msg GetForces()
            {
                return _wrench._force;
            }

            public Vector3Msg GetTorques()
            {
                return _wrench._torque;
            }

            public override string ToString()
            {
                return "WrenchStamped [header=" + _header.ToString() + ",  wrench=" + _wrench.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"header\" : " + _header.ToYAMLString() + ", \"wrench\" : " + _wrench.ToYAMLString() + "}";
            }
        }
    }
}