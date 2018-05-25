using SimpleJSON;

/* 
 *Duration Msg under std Msgs, written by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace std_msgs
    {
        public class DurationMsg : ROSBridgeMsg
        {
            private int _secs;
            private int _nsecs;

            public DurationMsg(JSONNode msg)
            {
                _secs = msg["sec"].AsInt;
                _nsecs = msg["nsecs"].AsInt;
            }

            public DurationMsg(int secs,int nsecs)
            {
                _secs = secs;
                _nsecs = nsecs;
            }

            public static string GetMessageType()
            {
                return "std_msgs/Duration";
            }

            public int GetSecs()
            {
                return _secs;
            }

            public int GetNsecs()
            {
                return _nsecs;
            }

            public override string ToString()
            {
                return "Duration [secs=" + _secs +", nsecs= " + _nsecs + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"secs\" : " + _secs + ", \"nsecs\" : " +_nsecs + "}";
            }
        }
    }
}