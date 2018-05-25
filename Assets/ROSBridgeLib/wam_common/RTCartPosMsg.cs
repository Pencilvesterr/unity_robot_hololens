using SimpleJSON;

/*  RTCartPos Msg, publisher to /wam/cart_pos_cmd
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. 
 *  By Cole Shing, 2017
 *  
 *  float32[3] position
 *  float32[3] rate_limits
 */

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class RTCartPosMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private float[] _position;
            private float[] _rate_lim;

            public RTCartPosMsg(JSONNode msg)
            {
                _position = new float[3]; //x y z
                for (int i = 0; i < _position.Length; i++)
                {
                    _position[i] = float.Parse(msg["position"][i]);
                }
                _rate_lim = new float[3];
                for (int i = 0; i < _rate_lim.Length; i++)
                {
                    _rate_lim[i] = float.Parse(msg["rate_limits"][i]);
                }
            }

            public RTCartPosMsg(float[] position, float[] rate_limits)
            {
                _position = position;
                _rate_lim = rate_limits;
            }

            public static string GetMessageType()
            {
                return "wam_common/RTCartPos";
            }

            public float[] GetPosition()
            {
                return _position;
            }

            public float[] GetRate_Limits()
            {
                return _rate_lim;
            }

            public override string ToString()
            {
                //converting the joints array into YAMLstring
                string positionarray = convert.floattoarray(_position);

                //converting the rate_limits array into YAMLstring
                string ratearray = convert.floattoarray(_rate_lim);

                return "RTCartPos [position=" + positionarray + ", rate_limits= " + ratearray + "]";
            }

            public override string ToYAMLString()
            {
                //converting the joints array into YAMLstring
                string positionarray = convert.floattoarray(_position);

                //converting the rate_limits array into YAMLstring
                string ratearray = convert.floattoarray(_rate_lim);

                return "{\"position\" : " + positionarray + ", \"rate_limits\" : " + ratearray + "}";
            }
        }
    }
}

