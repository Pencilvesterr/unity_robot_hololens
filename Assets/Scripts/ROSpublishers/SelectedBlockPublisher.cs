using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;


/*
 * Written by Steven Hoang 2020
 * RobotCommandPublisher controls the robot's action (execute, stop, get back to Ready State)
 * Previously, there are three different publishers, each dedicated for each task. This publisher is the combination of the three
 * Please get the latest version of virtual_barrier_ros on ROS to use this publisher
 */
namespace RosSharp.RosBridgeClient
{
    public class SelectedBlockPublisher : UnityPublisher<MessageTypes.Std.Int32>
    {
        private List<MessageTypes.Std.Int32> message_queue;
        public GameObject PublishGazeToggle;
        private Interactable PublishGazeToggleStatus;

        protected override void Start()
        {
            base.Start();
            message_queue = new List<MessageTypes.Std.Int32>();
            PublishGazeToggleStatus = PublishGazeToggle.GetComponent<Interactable>();
            PublishGazeToggleStatus.IsToggled = true;
        }
      
        private void Update()
        {
            if (message_queue.Count > 0)
            {
                // Publish the first message from queue
                Publish(message_queue[0]);
                // Then remove it
                message_queue.RemoveAt(0);
            }
        }
        public void PublishSelection(int data)
        {
            if (PublishGazeToggleStatus.IsToggled)
            {
                Debug.Log("Command Added to Queue '/gaze_object_selection': " + data.ToString());
                message_queue.Add(new MessageTypes.Std.Int32((short)data));
            }
        }
    }
}
