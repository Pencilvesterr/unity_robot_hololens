using SimpleJSON;

/*  RTJointVel Msg, publisher to /wam/jnt_vel_cmd
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. 
 *  By Cole Shing, 2017
 *  
 *  float32[] velocities
 */

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class RTJointVelMsg : ROSBridgeMsg
        {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private float[] _velocities;
            
            public RTJointVelMsg(JSONNode msg)
            {
                _velocities= new float[msg["velocities"].Count];
                for (int i = 0; i < _velocities.Length; i++)
                {
                    _velocities[i] = float.Parse(msg["velocities"][i]);
                }
            }

            public RTJointVelMsg(float[] velocities)
            {
                _velocities = velocities;
            }

            public static string GetMessageType()
            {
                return "wam_common/RTJointVel";
            }

            public float[] GetVelocities()
            {
                return _velocities;
            }
            
            public override string ToString()
            {
                //converting the velocities array into YAMLstring
                string velarray = convert.floattoarray(_velocities);

                return "RTJointVel [velocities=" + velarray + "]";
            }

            public override string ToYAMLString()
            {
                //converting the velocities array into YAMLstring
                string velarray = convert.floattoarray(_velocities);

                return "{\"velocities\" : " + velarray + "}";
            }
        }
    }
}

