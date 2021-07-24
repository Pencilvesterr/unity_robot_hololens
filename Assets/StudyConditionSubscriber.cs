
using RosSharp.RosBridgeClient.MessageTypes.CwsPlanning;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using RosSharp.RosBridgeClient.MessageTypes.Std;

/*
 * Written by Morgan Crouch 2020
 * Aims to update the zone and block selection of the robot in AR.
 */
namespace RosSharp.RosBridgeClient
{
 
    public class StudyConditionSubscriber : UnitySubscriber<MessageTypes.Std.Int32>
    {
        public GameObject RobotIntentDisplayed;
        public GameObject GazeSelectionPublished;
        public Material DefaultPlacementButtonMaterial;
        public Material DefaultZoneAndBlockMaterial;

        private Interactable RobotIntentDisplayedToggle;
        private Interactable GazeSelectionPublishedToggle;
        private GameObject[] PlacementButtons;
        private GameObject[] ZonesAndBlocks;
        
        // TODO: Get all zones and blocks, add their materials as args, set them when message received

        // 1: All, 2: Traffic Light Shown, 3: Eye Gaze Published, 4: None
        private int CurrentStudyCondition = 1;
        private bool isMessageReceived;
        protected override void Start()
        {
            base.Start();
            RobotIntentDisplayedToggle = RobotIntentDisplayed.GetComponent<Interactable>();
            GazeSelectionPublishedToggle = GazeSelectionPublished.GetComponent<Interactable>();
            PlacementButtons = GameObject.FindGameObjectsWithTag("PlacementButton");
            ZonesAndBlocks = GameObject.FindGameObjectsWithTag("HighlightableObject");

            isMessageReceived = false;
        }

        private void Update()
        {
            if (isMessageReceived)
            {
                ProcessMessage();
            }
        }

        private void ProcessMessage()
        {
            switch (CurrentStudyCondition)
            {
                case 1:
                    RobotIntentDisplayedToggle.IsToggled = true;
                    GazeSelectionPublishedToggle.IsToggled = true;
                    break;
                case 2:
                    RobotIntentDisplayedToggle.IsToggled = true;
                    GazeSelectionPublishedToggle.IsToggled = false;
                    break;
                case 3:
                    RobotIntentDisplayedToggle.IsToggled = false;
                    GazeSelectionPublishedToggle.IsToggled = true;
                    break;
                case 4:
                    RobotIntentDisplayedToggle.IsToggled = false;
                    GazeSelectionPublishedToggle.IsToggled = false;
                    break;
            }

            foreach (GameObject placementButton in PlacementButtons)
            {
                placementButton.GetComponent<MeshRenderer>().material = DefaultPlacementButtonMaterial;

            }

            foreach (GameObject zoneOrBlock in ZonesAndBlocks)
            {
                zoneOrBlock.GetComponent<MeshRenderer>().material = DefaultZoneAndBlockMaterial;
            }


            Debug.Log(string.Format("Robot intent displayed: {0}, Gaze Selection Published: {1}",
                RobotIntentDisplayedToggle.IsToggled, GazeSelectionPublishedToggle.IsToggled));
            
            isMessageReceived = false;
        }


        protected override void ReceiveMessage(Int32 message)
        {
            CurrentStudyCondition = message.data;
            isMessageReceived = true;
        }
    }
}
