using System.Collections;
using System.Text;
using SimpleJSON;

/*  Accel Msg   
 *  added to ROSBridgeLib and modified by Cole Shing, 2018
 *  
 *  geometry_msgs/Vector3 linear
 *  geometry_msgs/Vector3 angular
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class AccelMsg : ROSBridgeMsg
        {
            public Vector3Msg _linear;
            public Vector3Msg _angular;

            public AccelMsg(JSONNode msg)
            {
                _linear = new Vector3Msg(msg["linear"]);
                _angular = new Vector3Msg(msg["angular"]);
            }

            public AccelMsg(Vector3Msg l, Vector3Msg a)
            {
                _linear = l;
                _angular = a;
            }

            public static string getMessageType()
            {
                return "geometry_msgs/Accel";
            }

            public Vector3Msg GetLinearAccel()
            {
                return _linear;
            }

            public Vector3Msg GetAngularAccel()
            {
                return _angular;
            }

            public override string ToString()
            {
                return "geometry_msgs/Accel [linear=" + _linear.ToString() + ",  angular=" + _angular.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"linear\": " + _linear.ToYAMLString() + ", \"angular\": " + _angular.ToYAMLString() + "}";
            }
        }
    }
}
