using SimpleJSON;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using System; //for the boolean
using ROSBridgeLib.geometry_msgs;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ROSBridgeLib
{
    namespace wam_common
    {
        public class CameraPoseStampedMsg : ROSBridgeMsg
        {
            Vector3 cameraPos = new Vector3();
            Quaternion cameraOrien = new Quaternion();
            float[] convertArray;
            float[] convertRotation; 
            ConvertingArraytoString convert = new ConvertingArraytoString();

            public CameraPoseStampedMsg(JSONNode msg)
            {
                convertArray = new float[msg["cameraPose"].Count];
                for (int i = 0; i < convertArray.Length; i++)
                {
                    convertArray[i] = float.Parse(msg["cameraPose"][i]);
                }
                convertRotation = new float[msg["cameraOrientation"].Count];
                for (int i = 0; i < convertRotation.Length; i++)
                {
                    convertRotation[i] = float.Parse(msg["cameraOrientation"][i]);
                }

            }

            public CameraPoseStampedMsg(Vector3 cameraPosition, Quaternion cameraOrientation)
            {
                cameraPos = cameraPosition;
                cameraOrien = cameraOrientation;
            }

            public Vector3 getCameraPos()
            {
                return cameraPos;
            }

            public static string GetMessageType()
            {
                return "geometry_msgs/PoseStamped";
            }

            public float[] toFloatArray()
            {
   
                for(int i = 0; i < cameraPos.magnitude; i++ )
                {
                    convertArray[i] = cameraPos[i];
                }

                return convertArray;

            }
            public override string ToYAMLString()
            {
                //converting the joints array into YAMLstring
                string position = convert.floattoarray(convertArray);

                //converting the rate_limits array into YAMLstring
                string rotate = convert.floattoarray(convertRotation);
                
                //return "{\"header\" : " header.ToYAMLString() + ", frame_id=" + _frame_id + "}";
                return "{\"cameraPose\" : " + position + ", \"cameraOrientation\" : " + rotate + "}";
            }
        }
    }
}


