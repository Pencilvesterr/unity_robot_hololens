using SimpleJSON;

/*  RTCartVel Msg, publisher to /wam/cart_vel_cmd
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. 
 *  By Cole Shing, 2017
 *  
 *  float32[3] direction
 *  float32 magnitude
 */

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class RTCartVelMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private float[] _direction;
            private float _magnitude;

            public RTCartVelMsg(JSONNode msg)
            {
                _direction = new float[3];
                for (int i = 0; i < _direction.Length; i++)
                {
                    _direction[i] = float.Parse(msg["direction"][i]);
                }

                _magnitude = float.Parse(msg["magnitude"]);
            }

            public RTCartVelMsg(float[] direction, float magnitude)
            {
                _direction = direction;
                _magnitude = magnitude;
            }

            public static string GetMessageType()
            {
                return "wam_common/RTCartVel";
            }

            public float[] GetDirection()
            {
                return _direction;
            }

            public float GetMagnitude()
            {
                return _magnitude;
            }
            
            public override string ToString()
            {
                //converting the direction array into YAMLstring
                string directionarray = convert.floattoarray(_direction);

                return "RTCartVel [direction=" + directionarray + ", magnitude= " + _magnitude + "]";
            }

            public override string ToYAMLString()
            {
                //converting the direction array into YAMLstring
                string directionarray = convert.floattoarray(_direction);

                return "{\"direction\" : " + directionarray + ", \"magnitude\" : " + _magnitude + "}";
            }
        }
    }
}

