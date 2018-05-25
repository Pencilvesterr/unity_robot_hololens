using SimpleJSON;

/*  ChannelFloat32 Msg, 
 *  added to ROSBridgeLib by Cole Shing, 2017
 *  string name
 *  float32[] values
  */

namespace ROSBridgeLib
{
    namespace sensor_msgs
    {
        public class ChannelFloat32Msg : ROSBridgeMsg{

            ConvertingArraytoString convert = new ConvertingArraytoString(); //define conversion of arrays
            private string _name;
            private float[] _values;

            public ChannelFloat32Msg(JSONNode msg)
            {
                _name = msg["name"];
                _values = new float[msg["values"].Count];
                for (int i = 0; i < _values.Length; i++)
                {
                    _values[i] = float.Parse(msg["values"][i]);
                }
            }

            public ChannelFloat32Msg(string name, float[] values)
            {
                _name = name;
                _values = values;
            }

            public static string GetMessageType()
            {
                return "Sensor_msgs/ChannelFloat32";
            }

            public string GetName()
            {
                return _name;
            }

            public float[] GetValues()
            {
                return _values;
            }

            public override string ToString()
            {
                //converting the values array into string
                string valuesarray = convert.floattoarray(_values);

                return "RTJointPos [name=" + _name + ", values= " + valuesarray + "]";
            }

            public override string ToYAMLString()
            {
                //converting the rate_limits array into YAMLstring
                string valuesarray = convert.floattoarray(_values);

                return "{\"name\" : " + _name + ", \"values\" : " + valuesarray + "}";
            }
        }
    }
}

