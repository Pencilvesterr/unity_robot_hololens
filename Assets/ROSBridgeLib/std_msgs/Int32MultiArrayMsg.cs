using System.Collections;
using System.Text;
using SimpleJSON;

/* 
 * @brief ROSBridgeLib
 * @author Michael Jenkin, Robert Codd-Downey, Andrew Speers and Miquel Massot Campos
 * modified by Cole Shing, 2017
 */

namespace ROSBridgeLib {
	namespace std_msgs {
        public class Int32MultiArrayMsg : ROSBridgeMsg {
            ConvertingArraytoString convert = new ConvertingArraytoString();
            private MultiArrayLayoutMsg _layout;
            private int[] _data;

            public Int32MultiArrayMsg(JSONNode msg) {
                _layout = new MultiArrayLayoutMsg(msg["layout"]);
                _data = new int[msg["data"].Count];
				for (int i = 0; i < _data.Length; i++) {
                    _data[i] = int.Parse(msg["data"][i]);
                }
            }

            public Int32MultiArrayMsg(MultiArrayLayoutMsg layout, int[] data) {
                _layout = layout;
                _data = data;
            }

            public static string getMessageType() {
                return "std_msgs/Int32MultiArray";
            }

            public int[] GetData() {
                return _data;
            }

            public MultiArrayLayoutMsg GetLayout() {
                return _layout;
            }

            public override string ToString() {
                string array = convert.inttoarray(_data);
                return "Int32MultiArray [layout=" + _layout.ToString() + ", data=" + _data + "]";
            }

            public override string ToYAMLString() {
                string array = convert.inttoarray(_data);
                return "{\"layout\" : " + _layout.ToYAMLString() + ", \"data\" : " + array + "}";
            }
        }
    }
}