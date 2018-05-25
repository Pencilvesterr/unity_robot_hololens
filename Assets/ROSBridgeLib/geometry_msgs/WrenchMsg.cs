using System.Collections;
using System.Text;
using SimpleJSON;

/*  Wrench Msg   
 *  added to ROSBridgeLib and modified by Cole Shing, 2018
 *  
 *  geometry_msgs/Vector3 force
 *  geometry_msgs/Vector3 torque
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class WrenchMsg : ROSBridgeMsg
        {
            public Vector3Msg _force;
            public Vector3Msg _torque;

            public WrenchMsg(JSONNode msg)
            {
                _force = new Vector3Msg(msg["force"]);
                _torque = new Vector3Msg(msg["torque"]);
            }

            public WrenchMsg(Vector3Msg f, Vector3Msg t)
            {
                _force = f;
                _torque = t;
            }

            public static string getMessageType()
            {
                return "geometry_msgs/Wrench";
            }

            public Vector3Msg GetForces()
            {
                return _force;
            }

            public Vector3Msg GetTorques()
            {
                return _torque;
            }

            public override string ToString()
            {
                return "geometry_msgs/Wrench [force=" + _force.ToString() + ",  torque=" + _torque.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"force\": " + _force.ToYAMLString() + ", \"torque\": " + _torque.ToYAMLString() + "}";
            }
        }
    }
}
