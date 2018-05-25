using SimpleJSON;
using ROSBridgeLib.std_msgs;

/*  Accel Stamped Msg   
 *  added to ROSBridgeLib and modified by Cole Shing, 2018
 *  std_msgs/Header header
 *  Geometry_msgs/Accel accel
 *      geometry_msgs/Vector3 linear
 *      geometry_msgs/Vector3 angular
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class AccelStampedMsg : ROSBridgeMsg
        {
            public HeaderMsg _header;
            public AccelMsg _accel;

            public AccelStampedMsg(JSONNode msg)
            {
                _header = new HeaderMsg(msg["header"]);
                _accel = new AccelMsg(msg["accel"]);
            }

            public AccelStampedMsg(HeaderMsg header, AccelMsg accel)
            {
                _header = header;
                _accel = accel;
            }
            public static string GetMessageType()
            {
                return "geometry_msgs/AccelStamped";
            }

            public HeaderMsg GetHeader()
            {
                return _header;
            }

            public AccelMsg GetAccel()
            {
                return _accel;
            }

            public Vector3Msg GetLinearAccel()
            {
                return _accel._linear;
            }

            public Vector3Msg GetAngularAccel()
            {
                return _accel._angular;
            }

            public override string ToString()
            {
                return "AccelStamped [header=" + _header.ToString() + ",  accel=" + _accel.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"header\" : " + _header.ToYAMLString() + ", \"accel\" : " + _accel.ToYAMLString() + "}";
            }
        }
    }
}