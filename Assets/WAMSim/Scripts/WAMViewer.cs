using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using System; //for the boolean
using ROSBridgeLib.geometry_msgs;
using System.Collections.Generic;
using UnityEngine.UI;
using ROSBridgeLib.std_msgs;
using Vuforia;
using HoloToolkit.Unity;
/*
* Written by Cole Shing ,2017
* Simple viewer for talking to WAM. Currently when ran through Editor, the WAM
* will mimic the physical WAM and can be controlled using the 7 sliders that
* controls each joint angle. Use this as an example to start getting Rosbridge working for the WAM
* -if Barett hand is connected in unity will automatically subscribe to the hand's joint state and services
* To get this working, put this in the scene on any object
*/
// Joint angle limits
// J1 +/-2.6(150), J2 +/-2.0(113), J3 +/-2.8(157), 
// J4 +3.1/-0.9 (180/-50), J5 +1.24/-4.76(71/-273), 
// J6 +/-1.6(90), J7 +/-3.0(172)

/* position is in meter units, x positive is point away
 * from the cable side, y is perpendicular on a flat plane
 * z positive is the vertical height of the end effector.
 */

public class WAMViewer : MonoBehaviour
{
#if UNITY_EDITOR
    float[] sliders = { 0.0F, 0.0F, 0.0F, 1.57F, 0.0F, 0F, 0.0F }; //starting slider positions
#endif
    public Boolean Use_Sliders; //enable to allow control of the WAM using the sliders
    Boolean Handconnected; //flag to chheck if the hand is connected
    public Boolean Close_hand; //start the close gripper service
    public Boolean Open_hand; //start the open gripper service
    public Boolean startforce; //start the force torque tool service
    public Text debug_text;
    public Transform resultTemp;
    public Transform finalResult;
    //public Vector3 cam_wrt_WAM;
    private static int seq_id = 0;

    private static float[] home_angles = { 0.0F, -2.0F, 0.0F, 3.1F, 0.0F, 0F, 0.0F }; //the angles of the wam at home position
    private static float[] rate_lim = { 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F }; //the default rate limits

    private Boolean use_FT = true;

    private ROSBridgeWebSocketConnection ros = null; //defined in ROSBridgeWebSocketConnection
    // Define our subscribers, publishers and service response handlers
    private BHandServices bhandserv = new BHandServices(); //defining the barrett hand services list, has all the possible calls located in this class
    private WAMServices wamserv = new WAMServices(); //defines the wam service list, has all the possible calls
    
    void Start()
    {
#if !UNITY_EDITOR
            Use_Sliders = false;
#endif
        //creates the connection to the bridge
        //ros = new ROSBridgeWebSocketConnection("ws://137.82.173.74", 9090); //change to IP of ROS machine        
//        ros = new ROSBridgeWebSocketConnection("ws://137.82.173.72", 9090); //change to IP of ROS machine  
        ros = new ROSBridgeWebSocketConnection("ws://192.168.0.102", 9090); //change to IP of ROS machine  

        //add subscribers and publishers
        ros.AddSubscriber(typeof(WAMJointState)); //the joint state of the WAM
        ros.AddPublisher(typeof(WAMRTJointPos)); //used with sliders to send posiiton to the WAM
        ros.AddPublisher(typeof(CameraPosePublish)); //publish hololens pose relative to robot base
        ros.AddServiceResponse(typeof(WAMServiceResponse)); //the service call response

        if (GameObject.Find("BHand"))
        {
            Handconnected = true;
            ros.AddSubscriber(typeof(BHandJointState));  //subscribe to the topic joint_states of the barrett hand
            //ros.AddServiceResponse(typeof(BHandServiceResponse)); //the bhandservice response , however there should only be one service response
        }

        if (use_FT)
        {
            ros.AddSubscriber(typeof(WAMFTSensor));
            ros.AddSubscriber(typeof(WAMFTAccel));
        }

        bhandserv.ServInit(ros); //init the service class
        wamserv.ServInit(ros); //init the wam service
        ros.Connect(); //actually connects to the ros bridge
    }



    public void close_hand()

    {

        bhandserv.CloseGrasp();

    }

   public void open_hand()

    {

        bhandserv.OpenGrasp();

    }

    public void myoBandFeedbackVibrate(UInt8Msg data)
    {
        ros.Publish(myoBandFeedback.GetMessageTopic(), data);
    }


    public void move_to_pose(Vector3 position,Quaternion q)

    {

        PointMsg p = new PointMsg(position.x, position.y, position.z);
        QuaternionMsg q2 = new QuaternionMsg(q.x, q.y, q.z, q.w);

        PoseMsg send_pose = new PoseMsg(p, q2);

        wamserv.MoveToPose(send_pose);
        
    }



    public void lock_path(List<Vector3> path_points, List<Vector3> path_normals, int size)

    {
        
        List<Point32Msg> points= new List<Point32Msg>();
        List<Point32Msg> normals= new List<Point32Msg>();
        debug_text.text = "";
        for (int i=0; i < size; i++)
        {

            Point32Msg point = new Point32Msg(0, 0, 0);
            Point32Msg normal = new Point32Msg(0, 0, 0);
            point._x = path_points[i].z;
            point._y = -path_points[i].x;
            point._z = path_points[i].y;
           
            /*comment for user trials 
            debug_text.text += "("+ point._x + ","+ point._y+ "," + point._z+")"+", ";
            */
            points.Add(point);

            normal._x = path_normals[i].z;
            normal._y = -path_normals[i].x;
            normal._z = path_normals[i].y;

            normals.Add(normal);

        }
        wamserv.FollowPath(points,normals,size);

    }





    public void go_home(float[] joints)

    {
        wamserv.JointMove(joints);
    }


    public void free_path()

    {
        wamserv.StopVisualFix();
    }

    public void gravity()

    {
        wamserv.HoldJointPos(false);
    }

    public void myoVibrate(int level)
    {
        //function to send vibration 
         
    }


    // When application close, disconnect to ROS bridge
    void OnApplicationQuit()
    {
        if (ros != null)
            ros.Disconnect(); //extremely important to disconnect from ROS.OTherwise packets continue to flow
    }

    // Update is called once per frame in Unity. The Unity camera follows the robot (which is driven by
    // the ROS environment. 



    void publishPose(String topic, Vector3 p, Quaternion q)
    {
        PointMsg pMsg = new PointMsg(p[0], p[1], p[2]);
        QuaternionMsg qMsg = new QuaternionMsg(q[0], q[1], q[2], q[3]);

        PoseMsg pose = new PoseMsg(pMsg, qMsg);
        ros.Publish(topic, pose);
    }


    void Update()
    {
        /*
         * please leave the following commented out code; may need for future changes. 
        //Vector3 wamPos = GameObject.Find("ImageTarget").transform.position;
        Camera cameraPos = Camera.main;
        //Vector3 convert = cameraPos.ScreenToWorldPoint(GameObject.Find("ImageTarget").transform.position);
        Quaternion rotationInverse = Quaternion.Inverse(GameObject.Find("WAM").transform.rotation);
        Vector3 wamPos = GameObject.Find("WAM").transform.position * -1;
        Vector3 newPos = rotationInverse * wamPos;
        resultTemp.transform.SetPositionAndRotation(newPos, rotationInverse);
        finalResult.transform.SetPositionAndRotation((resultTemp.transform.rotation * cameraPos.transform.position) + resultTemp.transform.position, resultTemp.transform.rotation * cameraPos.transform.rotation);
        //Debug.Log("WAM's parent is:" + WAM.transform.parent.name);
        Debug.Log("Camera's parent is:" + cameraPos.transform.parent.name);
        
        float[] cameraPosArray = { finalResult.transform.position.x, finalResult.transform.position.y, finalResult.transform.position.z };
        PointMsg p = new PointMsg(cameraPosArray[0], cameraPosArray[1], cameraPosArray[2]);
        //PointMsg p = new PointMsg(cameraPosArray[0], cameraPosArray[1], cameraPosArray[2]);

        Quaternion cameraOrientation = Quaternion.Inverse(GameObject.Find("ImageTarget").transform.rotation);
        float[] cameraOrientationArray = { finalResult.transform.rotation.w, finalResult.transform.rotation.x, finalResult.transform.rotation.y, finalResult.transform.rotation.z };
        QuaternionMsg q = new QuaternionMsg(cameraOrientationArray[0], cameraOrientationArray[1], cameraOrientationArray[2], cameraOrientationArray[3]);
        
        DateTime now = DateTime.Now;
        int sec = now.Second;
        int nsec = now.Millisecond * 1000;
        TimeMsg time = new TimeMsg(sec, nsec);
        HeaderMsg header = new HeaderMsg(seq_id++, time, "base_link");
        
        PoseMsg pose = new PoseMsg(p, q);
        PoseStampedMsg msgForCam = new PoseStampedMsg(header, pose);
        ros.Publish(CameraPosePublish.GetMessageTopic(), pose);
        
          * The code here was what we were trying to do before; that is, trying to do many tranformations from one frame to another. Please keep just incase we use it again.
         // world to camera transform
         Quaternion worldToCameraRotation = worldToCameraTransform.rotation;
         Vector3 worldToCameraPos = worldToCameraTransform.position;
         // world to wam transform
         Quaternion worldToWAMRotation = worldToWAMTransform.rotation;
         Vector3 worldToWAMPos = worldToWAMTransform.position;
         // get inverse of world to wam transform (i.e., wam to world transform)
         Quaternion WAMToWorldRotation = Quaternion.Inverse(worldToWAMRotation);
         Vector3 WAMToWorldPos = -worldToWAMPos;

         // compute the transform from wam to camera
         Vector3 WAMToCameraPos = WAMToWorldRotation * worldToCameraPos + WAMToWorldPos;
         Quaternion WAMToCameraRotation = WAMToWorldRotation * worldToCameraRotation;

        
         float[] WAMToCameraPosArray = { WAMToCameraPos.x, WAMToCameraPos.y, WAMToCameraPos.z };
         PointMsg p = new PointMsg(WAMToCameraPosArray[0], WAMToCameraPosArray[1], WAMToCameraPosArray[2]); //flip because we want RHS, not LHS (reference: https://answers.unity.com/storage/temp/12048-lefthandedtorighthanded.pdf)

         float[] WAMToCameraOrientationArray = { WAMToCameraRotation.x, WAMToCameraRotation.y, WAMToCameraRotation.z, WAMToCameraRotation.w };
         QuaternionMsg q = new QuaternionMsg(WAMToCameraOrientationArray[0], WAMToCameraOrientationArray[1], WAMToCameraOrientationArray[2], WAMToCameraOrientationArray[3]);
         

       
        float[] WAMToCameraPosArray = { worldToCameraPos.x, worldToCameraPos.y, worldToCameraPos.z };
        PointMsg p = new PointMsg(WAMToCameraPosArray[0], WAMToCameraPosArray[1], WAMToCameraPosArray[2]); //flip because we want RHS, not LHS (reference: https://answers.unity.com/storage/temp/12048-lefthandedtorighthanded.pdf)

        float[] WAMToCameraOrientationArray = { worldToCameraRotation.x, worldToCameraRotation.y, worldToCameraRotation.z, worldToCameraRotation.w };
        QuaternionMsg q = new QuaternionMsg(WAMToCameraOrientationArray[0], WAMToCameraOrientationArray[1], WAMToCameraOrientationArray[2], WAMToCameraOrientationArray[3]);
        Transform worldToCameraTransform = Camera.main.transform;
         ros.Publish(CameraPosePublish.GetMessageTopic(), msgForCam);
        */


        Transform worldToWAMTransform = GameObject.Find("WAM").transform;
        Vector3 cam_wrt_WAM = new Vector3();
        cam_wrt_WAM = worldToWAMTransform.InverseTransformPoint(Camera.main.transform.position);
        cam_wrt_WAM = cam_wrt_WAM * 0.001f;
        cam_wrt_WAM.y = cam_wrt_WAM.y - 0.354f;

        Vector3 cam_wrt_WAM_final = new Vector3(); //Applying the final position change needed to get the correct x,y,z coordinates. Stored in cam_wrt_WAM_final
        
        cam_wrt_WAM_final.x = cam_wrt_WAM.z;
        cam_wrt_WAM_final.y = -cam_wrt_WAM.x;
        cam_wrt_WAM_final.z = cam_wrt_WAM.y;

 

        publishPose(CameraPosePublish.GetMessageTopic(), cam_wrt_WAM_final, Camera.main.transform.rotation);





        
#if UNITY_EDITOR
        if (Use_Sliders) //if selected control the 7 joints of the wam using sliders
        {
            float[] joints = new float[7];
            float[] rate_limits = new float[7];

            for (int i = 0; i < 7; i++)
            {
                joints[i] = sliders[i]; //set joints to the sliders position
                rate_limits[i] = System.Convert.ToSingle(0.1); //set rate to be 0.1 always
            }
            //creates the msg to publish to send control the WAM
            RTJointPosMsg msg = new RTJointPosMsg(joints, rate_limits); //creates the RTJointPos msg defined in the same name
            ros.Publish(WAMRTJointPos.GetMessageTopic(), msg); //publish the message
        }
#endif
        if(Open_hand) //if selected open the barrett hand
        {
            bhandserv.OpenGrasp();
            Open_hand = false;
        }
        if (Close_hand) //if selected close the barrett hand
        {
            bhandserv.CloseGrasp();
            Close_hand = false;
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


        if (startforce) //simple force torque application of the wam
        {
            float[] forces = { 0f, 0f, 0f };
            float[] torque = { 0f, 0f, 0f };
            wamserv.ForceTorqueTool(forces, torque);
            startforce = false;
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