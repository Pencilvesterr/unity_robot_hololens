using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Written by Steven Hoang 2020
 * RobotCommandPublisher controls the robot's action (execute, stop, get back to Ready State)
 * Previously, there are three different publishers, each dedicated for each task. This publisher is the combination of the three
 * Please get the latest version of virtual_barrier_ros on ROS to use this publisher
 */
namespace RosSharp.RosBridgeClient
{
    public class RobotCommandPublisher : UnityPublisher<MessageTypes.Std.Int16>
    {
        private List<MessageTypes.Std.Int16> message_queue;
        public const int EXECUTE_TRIGGER = 1;
        public const int STOP_TRIGGER = 2;
        public const int READY_STATE_TRIGGER = 3;

        protected override void Start()
        {
            base.Start();
            InitialisedMessage();
        }
        private void InitialisedMessage()
        {
            message_queue = new List<MessageTypes.Std.Int16>();
        }
        private void Update()
        {
            // If there is message in the queue
            if (message_queue.Count > 0)
            {
                // Publish the first message from queue
                Publish(message_queue[0]);
                // Then remove it
                message_queue.RemoveAt(0);
            }
        }
        public void SendCommand(int data)
        {
            Debug.Log("Command Added to Queue");
            message_queue.Add(new MessageTypes.Std.Int16((short)data));
        }
    }
}
