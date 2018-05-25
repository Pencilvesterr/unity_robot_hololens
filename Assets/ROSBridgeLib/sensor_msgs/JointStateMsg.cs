using SimpleJSON;
using ROSBridgeLib.std_msgs;

/*  JointState Msg, subscriber to /wam/joint_states
 *  added to ROSBridgeLib. To get the messages working for ROS WAM. By Cole Shing, 2017
 *  std_msgs/ Header header
 *  string[] name
 *  float64[] position
 *  float64[] velocity
 *  float64[] effort
 */

namespace ROSBridgeLib
{
    namespace sensor_msgs
    {
        public class JointStateMsg : ROSBridgeMsg {

            ConvertingArraytoString convert = new ConvertingArraytoString();
            private HeaderMsg _header;
            private string[] _name;
            private double[] _position;
            private double[] _velocity;
            private double[] _effort;

            public JointStateMsg(JSONNode msg){
                _header = new HeaderMsg(msg["header"]);
                _name = new string[msg["name"].Count];
                for (int i = 0; i < _name.Length; i++)
                {
                    _name[i] = (msg["name"][i]);
                }
                _position = new double[msg["position"].Count];
                for (int i = 0; i < _position.Length; i++)
                {
                    _position[i] = double.Parse(msg["position"][i]);
                }
                _velocity = new double[msg["velocity"].Count];
                for (int i = 0; i < _velocity.Length; i++)
                {
                    _velocity[i] = double.Parse(msg["velocity"][i]);
                }
                _effort = new double[msg["effort"].Count];
                for (int i = 0; i < _effort.Length; i++)
                {
                    _effort[i] = double.Parse(msg["effort"][i]);
                }
            }

            public JointStateMsg(HeaderMsg header, string[] name, double[] position, double[] velocity, double[] effort){
                _header = header;
                _name = name;
                _position = position;
                _velocity = velocity;
                _effort = effort;
            }

            public static string GetMessageType(){
                return "sensor_msgs/JointState";
            }

            public string[] GetName(){
                return _name;
            }

            public double[] GetPosition(){
                return _position;
            }

            public double[] GetVelocity(){
                return _velocity;
            }

            public double[] GetEffort(){
                return _effort;
            }

            public override string ToString(){
                //converting the name array into string
                string namearray = convert.stringtoarray(_name);

                //converting the position array into string
                string posarray = convert.doubletoarray(_position);

                //converting the velocity array into string
                string velarray = convert.doubletoarray(_velocity);

                //converting the effort array into string
                string effarray = convert.doubletoarray(_effort);

                return "JointState [header=" + _header.ToString() + ", name= " + namearray + ",  position=" + posarray
                    + ",  velocity=" + velarray + ",  effort=" + effarray + "]";
            }

            public override string ToYAMLString(){

                //converting the name array into YAML string
                string namearray = convert.stringtoarray(_name);

                //converting the position array into YAML string
                string posarray = convert.doubletoarray(_position);

                //converting the velocity array into YAML string
                string velarray = convert.doubletoarray(_velocity);

                //converting the effort array into YAML string
                string effarray = convert.doubletoarray(_effort);

                return "{\"header\" : " + _header.ToYAMLString() + ", \"name\" : " + namearray + ", \"position\" : " + posarray
                    + ", \"velocity\" : " + velarray + ", \"effort\" : " + effarray + "}";
            }
        }
    }
}