using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using RosSharp.RosBridgeClient;
using RosSharp;
using Microsoft.MixedReality.Toolkit.UI;
using System;

/* Written by Steven Lay and Steven Hoang, 2020.
 * Methods for the voice commands are stored here.
 * Based off the original RobotSpeechManager modified by Caris Lab students.
 * Remove all Caris Lab student code unused by us.
 * Modified on 19/07/2020 by Steven Hoang with the new ROS-sharp integration
 */
public class VoiceCommands : MonoBehaviour
{
    // Make empty object that is location of primary pointer
    private GameObject pointerPos;

    // Set execution mode suitable for Steven's user study
    [SerializeField]
    private bool stevensExperiment = false;

    // Prefab for Set Point ball
    [SerializeField]
    private Transform spherePoint = null;

    // Prefab for Cube Barrier
    [SerializeField]
    private Transform cubeBarrierPrefab = null;

    // Prefab for Sphere Barrier
    [SerializeField]
    private Transform sphereBarrierPrefab = null;

    // Virtual person barrier
    [SerializeField]
    private BarrierPerson barrierPerson = null;

    // Create a line for use in displaying planned trajectory from MoveIt
    private LineRenderer lineConnecting;

    // ROS Connector to communicate with ROS
    private GameObject ROSConnector;
    // Robot Transform
    private GameObject RobotModel;
    private Transform ros_world_coord_frame;

    private List<Vector3> pathPoints = new List<Vector3>();

    public static int barrierCount { get; set; } = 0;

    private Vector3 EEF_Offset = new Vector3(0, 0.16f, 0);

    private Boolean usingGaze;

    void Start()
    {
        // Find ROS Connector to communicate with ROS side
        ROSConnector = GameObject.Find("ROS Connector");
        // Find the RobotModel and obtain its "world" coordinate frame
        RobotModel = GameObject.FindGameObjectWithTag("Robot");
        ros_world_coord_frame = RobotModel.transform.Find("world").transform;

        pointerPos = new GameObject("pointerPos");
        CoreServices.InputSystem?.FocusProvider?.SubscribeToPrimaryPointerChanged(OnPrimaryPointerChanged, true);

        // Set the default tag for all spawned setPoint prefab instances for easy removal later on.
        spherePoint.tag = "clone";
    }

    // From Primary Pointer MRTK example, we need this to track the Primary pointer and place the pointerPos on it
    private void OnPrimaryPointerChanged(IMixedRealityPointer oldPointer, IMixedRealityPointer newPointer)
    {
        if (pointerPos != null)
        {
            if (newPointer != null)
            {
                Transform parentTransform = newPointer.BaseCursor?.GameObjectReference?.transform;

                // If there's no cursor try using the controller pointer transform instead
                if (parentTransform == null)
                {
                    var controllerPointer = newPointer as BaseControllerPointer;
                    parentTransform = controllerPointer?.transform;
                }

                if (parentTransform != null)
                {
                    pointerPos.transform.SetParent(parentTransform, false);
                    pointerPos.SetActive(true);
                    pointerPos.transform.position = parentTransform.position;
                    return;
                }
            }

            pointerPos.SetActive(false);
            pointerPos.transform.SetParent(null, false);
        }
    }

    // If the Robot is misaligned wrt the Image Marker, attempts to one-shot recalibrate it
    public void Recalibrate()
    {
        GameObject.Find("ImageTarget").GetComponent<CustomDefaultTrackableEventHandler>().recalibrate = true;
        GameObject.Find("ImageTarget").GetComponent<CustomDefaultTrackableEventHandler>().reActivate();
    }

    // Destroys all points and barriers visible
    public void ClearAll()
    {
        ClearRoutine(true, true);
    }

    public void ClearBarriers()
    {
        ClearRoutine(false, true);
    }

    public void ClearPoints()
    {
        ClearRoutine(true, false);
    }

    private void ClearRoutine(bool clearPoints, bool clearBarriers)
    {
        if (clearPoints)
        {
            pathPoints.Clear();
            var clones = GameObject.FindGameObjectsWithTag("clone");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
            // Clear the displayed trajectory line
            // ROSConnector.GetComponent<TrajectoryLineDisplay>().ClearTrajectoryLine();
        }

        if (clearBarriers)
        {
            List<string> barrierNames = new List<string>();
            var bars = GameObject.FindGameObjectsWithTag("Barrier");
            foreach (var bar in bars)
            {
                Destroy(bar);
            }
            barrierCount = 0;
        }
    }

    // Sends goal points to MoveIt for a trajectory to be planned
    public void LockPath()
    {
        List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose> goal_points = new List<RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose>();
        // Hardcoded orientation of the EEF, gonna look for a better way to configure this
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion quaternion = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion
                                                                                                            (0.923956f, -0.382499f, 0, 0);
        // Construct array of goalpoints
        foreach (var point in pathPoints)
        {
            RosSharp.RosBridgeClient.MessageTypes.Geometry.Point position = GetGeometryPoint(point.Unity2Ros());
            goal_points.Add(new RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose(position, quaternion));
        }
        var pointsObjects = GameObject.FindGameObjectsWithTag("clone");
        foreach (var point in pointsObjects)
        {
            point.GetComponent<MeshRenderer>().material.color = Color.blue; 
        }
        ROSConnector.GetComponent<PathRequest>().SendRequest(goal_points);
    }

    // Causes Robot to begin following trajectory path planned
    public void Execute()
    {
        // barrierPerson.Hide = false;

        if (stevensExperiment)
        {
            // Do experimental procedure here
        }
        else
        {
            ROSConnector.GetComponent<RobotCommandPublisher>().SendCommand(RobotCommandPublisher.EXECUTE_TRIGGER);
        }
    }

    // Requests for the Robot to stop
    public void Terminate()
    {
        // barrierPerson.Hide = true;
        ROSConnector.GetComponent<RobotCommandPublisher>().SendCommand(RobotCommandPublisher.STOP_TRIGGER);
    }

    // Requests for the Robot to move to Ready State
    public void ReadyState()
    {
        // barrierPerson.Hide = true;
        ROSConnector.GetComponent<RobotCommandPublisher>().SendCommand(RobotCommandPublisher.READY_STATE_TRIGGER);
    }

    // Set the Robot mode to preview control
    public void Preview()
    {
        // ROSConnector.GetComponent<TrajectoryActionDisplay>().PreviewStart();
    }

    // Sets a goal point at the current gaze position, must be on workspace
    public void SetPoint()
    {
        Transform child;

        if (usingGaze)
        {
            GameObject focusedObject = CoreServices.InputSystem.GazeProvider.GazeTarget;
            if (focusedObject == null || focusedObject.tag != "Workspace")
            {
                Debug.Log("Barrier not set on workspace, backing out");
                return;
            }

            child = Instantiate(spherePoint, CoreServices.InputSystem.GazeProvider.HitPosition, Quaternion.identity);
        }
        else
        {
            GameObject focusedObject = CoreServices.InputSystem?.FocusProvider?.GetFocusedObject(CoreServices.InputSystem?.FocusProvider?.PrimaryPointer);
            if (focusedObject == null || focusedObject.tag != "Workspace")
            {
                Debug.Log("Barrier not set on workspace, backing out");
                return;
            }

            child = Instantiate(spherePoint, pointerPos.transform.position, Quaternion.identity);
        }

        pathPoints.Add(ros_world_coord_frame.InverseTransformPoint(child.position) + EEF_Offset);

        Debug.LogFormat("Adding point {0} at {1}", pathPoints.Count, ros_world_coord_frame.InverseTransformPoint(child.position).ToString("F3"));
    }

    public void ClearLastPoint()
    {
        var clones = GameObject.FindGameObjectsWithTag("clone");

        if (clones.Length > 0)
        {
            Destroy(clones[clones.Length - 1]);
        }
    }

    public void AddCubeBarrier()
    {
        AddBarrier(0);
    }

    public void AddSphereBarrier()
    {
        AddBarrier(1);
    }

    public void HandRaysOff()
    {
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
        PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOn);

        usingGaze = true;
    }

    public void HandRaysOn()
    {
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.Default);
        PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOff);

        usingGaze = false;
    }

    /* Adds a Cube or Sphere primitive barrier at the current gaze position, must be on workspace
     * int type: 0 = Cube, 1 = Sphere
     */
    private void AddBarrier(int type)
    {
        Transform child;
        Transform barrierType = (type == 0) ? cubeBarrierPrefab : sphereBarrierPrefab;

        if (usingGaze)
        {
            GameObject focusedObject = CoreServices.InputSystem.GazeProvider.GazeTarget;
            if (focusedObject == null || focusedObject.tag != "Workspace")
            {
                Debug.Log("Barrier not set on workspace, backing out");
                return;
            }

            child = Instantiate(barrierType, CoreServices.InputSystem.GazeProvider.HitPosition, Quaternion.identity);
        }
        else
        {
            GameObject focusedObject = CoreServices.InputSystem?.FocusProvider?.GetFocusedObject(CoreServices.InputSystem?.FocusProvider?.PrimaryPointer);
            if (focusedObject == null || focusedObject.tag != "Workspace")
            {
                Debug.Log("Barrier not set on workspace, backing out");
                return;
            }

            child = Instantiate(barrierType, pointerPos.transform.position, Quaternion.identity);
        }

        child.tag = "Barrier";
        child.name = "Barrier" + barrierCount++;
        child.transform.parent = null;
        child.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        child.transform.parent = ros_world_coord_frame;

        Debug.Log("Adding barrier");
    }

    // Remove barrier currently gazed at
    public void RemoveBarrier()
    {
        GameObject GazedObj = CoreServices.InputSystem.GazeProvider.GazeTarget;

        if (GazedObj.CompareTag("Barrier"))
        {
            Destroy(GazedObj);
            barrierCount--;
        }
    }
    public RosSharp.RosBridgeClient.MessageTypes.Geometry.Point GetGeometryPoint(Vector3 position)
    {
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Point geometryPoint = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Point();
        geometryPoint.x = position.x;
        geometryPoint.y = position.y;
        geometryPoint.z = position.z;
        return geometryPoint;
    }
    public RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
    {
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion geometryQuaternion = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion();
        geometryQuaternion.x = quaternion.x;
        geometryQuaternion.y = quaternion.y;
        geometryQuaternion.z = quaternion.z;
        geometryQuaternion.w = quaternion.w;
        return geometryQuaternion;
    }
}
