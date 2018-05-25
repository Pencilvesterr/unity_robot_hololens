using SimpleJSON;

/*  RTJointPos Msg, publisher to /wam/jnt_pos_cmd
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. 
 *  By Cole Shing, 2017
 *  
 *  float32[] joints
 *  float32[] rate_limits
 */

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class RTJointPosMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private float[] _joints;
            private float[] _rate_lim;

            public RTJointPosMsg(JSONNode msg)
            {
                _joints = new float[msg["joints"].Count];
                for (int i = 0; i < _joints.Length; i++)
                {
                    _joints[i] = float.Parse(msg["joints"][i]);
                }
                _rate_lim = new float[msg["rate_limits"].Count];
                for (int i = 0; i < _rate_lim.Length; i++)
                {
                    _rate_lim[i] = float.Parse(msg["rate_limits"][i]);
                }
            }

            public RTJointPosMsg(float[] joints, float[] rate_limits)
            {
                _joints = joints;
                _rate_lim = rate_limits;
            }

            public static string GetMessageType()
            {
                return "wam_common/RTJointPos";
            }

            public float[] GetJoints()
            {
                return _joints;
            }

            public float[] GetRate_Limits()
            {
                return _rate_lim;
            }

            public override string ToString()
            {
                //converting the joints array into YAMLstring
                string jointarray = convert.floattoarray(_joints);

                //converting the rate_limits array into YAMLstring
                string ratearray = convert.floattoarray(_rate_lim);

                return "RTJointPos [joints=" + jointarray + ", rate_limits= " + ratearray + "]";
            }

            public override string ToYAMLString()
            {
                //converting the joints array into YAMLstring
                string jointarray = convert.floattoarray(_joints);

                //converting the rate_limits array into YAMLstring
                string ratearray = convert.floattoarray(_rate_lim);

                return "{\"joints\" : " + jointarray + ", \"rate_limits\" : " + ratearray + "}";
            }
        }
    }
}

