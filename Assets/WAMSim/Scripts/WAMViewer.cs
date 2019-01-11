/*
Copyright Caris Lab
Author: Adnan Karim
Reorgranizing the WamViewer file to clean up a bit
*/

using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using System; 
using ROSBridgeLib.geometry_msgs;
using System.Collections.Generic;
using UnityEngine.UI;
using ROSBridgeLib.std_msgs;
using Vuforia;
using HoloToolkit.Unity;

public class WamViewer : MonoBehaviour
{
    #if UNITY_EDITOR
        float[] sliders = { 0.0F, 0.0F, 0.0F, 1.57F, 0.0F, 0F, 0.0F }; //starting slider positions
    #endif
        public Boolean useSliders;
        Boolean handConnected;
        public Boolean closeHand;
        public Boolean openHand;
        public Boolean startForce;
        public Text debugText;
        public Transform initialResult;
        public Transform finalResult;
        private static int sequenceId = 0;

        private static float[] homeAngles = { 0.0F, -2.0F, 0.0F, 3.1F, 0.0F, 0F, 0.0F }; 
        private static float[] defaultRateLimits = { 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F }; 

        private Boolean useFt = true;

        private ROSBridgeWebSocketConnection rosWebSocketConnection = null; //defined in ROSBridgeWebSocketConnection
        // Define our subscribers, publishers and service response handlers
        private BHandServices bHandServices = new BHandServices(); //defining the barrett hand services list, has all the possible calls located in this class
        private WAMServices wamServices = new WAMServices(); //defines the wam service list, has all the possible calls
        
        void Start()
        {
            usingUnityEditor();
            setUpRosWebSocketConnection();
            setUpBarrettHand();
            setUpFT(useFt);
            initializeAllServices();
            rosWebSocketConnection.Connect();


        }

        public void closeBarrettHand()
        {
            bHandServices.CloseGrasp();
        }

        public void openBarettHand()
        {
            bHandServices.OpenGrasp();
        }

        public void myoBandFeedbackVibrate(UInt8Msg data)
        {
            rosWebSocketConnection.Publish(myoBandFeedback.GetMessageTopic(), data);
        }

        public void moveToPose(Vector3 position, Quaternion quaternion)
        {
            PointMsg pointMessage = new PointMsg(position.x, position.y, position.z);
            QuaternionMsg quaternionMessage = new QuaternionMsg(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            PoseMsg pose = new PoseMsg(pointMessage, quaternionMessage);
            wamServices.MoveToPose(pose);
        }

        public void lockPath(List<Vector3> pointsOfPath, List<Vector3> normalOfPath, int lengthOfPath)
        {
            List<Point32Msg> listOfPoints= new List<Point32Msg>();
            List<Point32Msg> listOfNormals= new List<Point32Msg>();
            debugText.text = "";
            for (int i=0; i < lengthOfPath; i++)
            {

                Point32Msg point = new Point32Msg(0, 0, 0);
                Point32Msg normal = new Point32Msg(0, 0, 0);
                point._x = pointsOfPath[i].z;
                point._y = -pointsOfPath[i].x;
                point._z = pointsOfPath[i].y;
               
                listOfPoints.Add(point);

                normal._x = normalOfPath[i].z;
                normal._y = -normalOfPath[i].x;
                normal._z = normalOfPath[i].y;

                listOfNormals.Add(normal);

            }
            wamServices.FollowPath(listOfPoints, listOfNormals, lengthOfPath);
        }

        public void goHome(float[] joints)

        {
            wamServices.JointMove(joints);
        }


        public void freePath()

        {
            wamServices.StopVisualFix();
        }

        public void gravity()

        {
            wamServices.HoldJointPos(false);
        }

        public void myoVibrate(int levelOfVibration)
        {
            //TODO: function to send vibration 
             
        }

        void OnApplicationQuit()
        {
            if(rosWebSocketConnection != null)
            {
                rosWebSocketConnection.Disconnect();
            }
        }

        void Update()
        {

            Transform worldToWAMTransform = GameObject.Find("WAM").transform;
            Vector3 cameraWithRespectToWAM = new Vector3();
            cameraWithRespectToWAM = worldToWAMTransform.InverseTransformPoint(Camera.main.transform.position);
            cameraWithRespectToWAM = cameraWithRespectToWAM * 0.001f;
            cameraWithRespectToWAM.y = cameraWithRespectToWAM.y - 0.354f;

            Vector3 cameraWithRespectToWAMFinal = new Vector3();
            
            cameraWithRespectToWAMFinal.x = cameraWithRespectToWAM.z;
            cameraWithRespectToWAMFinal.y = -cameraWithRespectToWAM.x;
            cameraWithRespectToWAMFinal.z = cameraWithRespectToWAM.y;

            

            publishPose(CameraPosePublish.GetMessageTopic(), cameraWithRespectToWAMFinal, Camera.main.transform.rotation);
        }
        /*
        Helper Functions
        */
        void usingUnityEditor()
        {
            #if !UNITY_EDITOR
                useSliders = false;
            #endif

            #if UNITY_EDITOR
                float[] sliderValues = { 0.0F, 0.0F, 0.0F, 1.57F, 0.0F, 0F, 0.0F }; 
            #endif

        }
        void setUpRosWebSocketConnection()
        {
            rosWebSocketConnection = new ROSBridgeWebSocketConnection("ws://192.168.0.102", 9090);
            rosWebSocketConnection.AddSubscriber(typeof(WAMJointState));
            rosWebSocketConnection.AddPublisher(typeof(WAMRTJointPos)); 
            rosWebSocketConnection.AddPublisher(typeof(CameraPosePublish));
            rosWebSocketConnection.AddServiceResponse(typeof(WAMServiceResponse)); 
        }

        void setUpBarrettHand()
        {
            if (GameObject.Find("BHand"))
            {
                handConnected = true;
                rosWebSocketConnection.AddSubscriber(typeof(BHandJointState));
            }
        }

        void setUpFt(Boolean useFt)
        {
            if(useFT)
            {
                rosWebSocketConnection.AddSubscriber(typeof(WAMFTSensor));
                rosWebSocketConnection.AddSubscriber(typeof(WAMFTAccel));
            }
        }

        void initializeAllServices()
        {
            bHandServices.ServInit(rosWebSocketConnection);
            wamServices.ServInit(rosWebSocketConnection); 
        }

        void publishPose(String topic, Vector3 point, Quaternion quaternion)
        {
            PointMsg pointMessage = new PointMsg(point.x, point.y, point.z);
            QuaternionMsg quaternionMessage = new QuaternionMsg(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            PoseMsg poseToPublish = new PoseMsg(pointMessage, quaternionMessage);
            rosWebSocketConnection.Publish(poseToPublish);
        }

        #if UNITY_EDITOR
                if (useSliders) //if selected control the 7 joints of the wam using sliders
                {
                    float[] joints = new float[7];
                    float[] defaultRateLimits = new float[7];

                    for (int i = 0; i < 7; i++)
                    {
                        joints[i] = sliders[i]; //set joints to the sliders position
                        defaultRateLimits[i] = System.Convert.ToSingle(0.1); //set rate to be 0.1 always
                    }
                    //creates the msg to publish to send control the WAM
                    RTJointPosMsg msg = new RTJointPosMsg(joints, defaultRateLimits); //creates the RTJointPos msg defined in the same name
                    ros.Publish(WAMRTJointPos.GetMessageTopic(), msg); //publish the message
                }
        #endif
                if(openHand) //if selected open the barrett hand
                {
                    bhandserv.OpenGrasp();
                    openHand = false;
                }
                if (closeHand) //if selected close the barrett hand
                {
                    bhandserv.CloseGrasp();
                    closeHand = false;
                }

        /*
                if (pose_activate) //simple force torque application of the wam
                {
                    PoseMsg test;
                    test.

                    float[] forces = { 0f, 0f, 0f };
                    float[] torque = { 0f, 0f, 0f };
                    wamserv.ForceTorqueTool(forces, torque);
                    startforce = false;
                }

            */


                if (startForce) //simple force torque application of the wam
                {
                    float[] forces = { 0f, 0f, 0f };
                    float[] torque = { 0f, 0f, 0f };
                    wamserv.ForceTorqueTool(forces, torque);
                    startForce = false;
                }

                ros.Render(); //pretty much same as ros.spin()
            }

        #if UNITY_EDITOR
            void OnGUI() //creates the sliders onto the gui in editor
            {
                sliders[0] = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), sliders[0], -2.6F, 2.6F);
                sliders[1] = GUI.HorizontalSlider(new Rect(25, 55, 100, 30), sliders[1], -2.0F, 2.0F);
                sliders[2] = GUI.HorizontalSlider(new Rect(25, 85, 100, 30), sliders[2], -2.8F, 2.8F);
                sliders[3] = GUI.HorizontalSlider(new Rect(25, 115, 100, 30), sliders[3], -0.9F, 3.1F);
                sliders[4] = GUI.HorizontalSlider(new Rect(25, 145, 100, 30), sliders[4], -4.76F, 1.24F);
                sliders[5] = GUI.HorizontalSlider(new Rect(25, 175, 100, 30), sliders[5], -1.6F, 1.6F);
                sliders[6] = GUI.HorizontalSlider(new Rect(25, 205, 100, 30), sliders[6], -3.0F, 3.0F);
                
            }
        #endif

}