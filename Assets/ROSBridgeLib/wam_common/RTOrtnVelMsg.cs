using SimpleJSON;

/*  RTOrtnVel Msg, publisher to /wam/ortn_vel_cmd
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. 
 *  By Cole Shing, 2017
 *  
 *  float32[3] angular
 *  float32 magnitude
 */

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class RTOrtnVelMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private float[] _angular;
            private float _magnitude;

            public RTOrtnVelMsg(JSONNode msg)
            {
                _angular = new float[3];
                for (int i = 0; i < _angular.Length; i++)
                {
                    _angular[i] = float.Parse(msg["angular"][i]);
                }

                _magnitude = float.Parse(msg["magnitude"]);
            }

            public RTOrtnVelMsg(float[] angular, float magnitude)
            {
                _angular = angular;
                _magnitude = magnitude;
            }

            public static string GetMessageType()
            {
                return "wam_common/RTOrtnVel";
            }

            public float[] GetAngular()
            {
                return _angular;
            }

            public float GetMagnitude()
            {
                return _magnitude;
            }

            public override string ToString()
            {
                //converting the angular array into YAMLstring
                string angulararray = convert.floattoarray(_angular);

                return "RTJointVel [angular=" + angulararray + ", magnitude= " + _magnitude + "]";
            }

            public override string ToYAMLString()
            {
                //converting the angular array into YAMLstring
                string angulararray = convert.floattoarray(_angular);

                return "{\"angular\" : " + angulararray + ", \"magnitude\" : " + _magnitude + "}";
            }
        }
    }
}

