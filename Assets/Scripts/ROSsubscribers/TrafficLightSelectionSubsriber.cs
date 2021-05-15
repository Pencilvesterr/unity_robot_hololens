
using RosSharp.RosBridgeClient.MessageTypes.CwsPlanning;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

/*
 * Written by Morgan Crouch 2020
 * Aims to update the zone and block selection of the robot in AR.
 */
namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(AudioSource))]
    public class TrafficLightSelectionSubsriber : UnitySubscriber<MessageTypes.CwsPlanning.TrafficLight>
    {
        AudioSource audio_source;

        public Material default_material;
        public Material yellow_highlight;
        public Material red_highlight;
        public AudioClip transition_sound;

        public GameObject ARRobotIntentButton;
        private Interactable ToggleStatus;

        private int block_selected;
        private int block_status;
        private int zone_selected;
        private int zone_status;
        private GameObject[] highlightable_objects;
        private GameObject current_zone;
        private GameObject current_block;


        private bool isMessageReceived;
        protected override void Start()
        {
            base.Start();
            highlightable_objects = GameObject.FindGameObjectsWithTag("HighlightableObject");
            audio_source = GetComponent<AudioSource>();
            ToggleStatus = ARRobotIntentButton.GetComponent<Interactable>();
            isMessageReceived = false;
            ToggleStatus.IsToggled = true;
        }
        protected override void ReceiveMessage(TrafficLight message)
        {
            block_selected = message.block_selected;
            block_status = message.block_status;
            zone_selected = message.zone_selected;
            zone_status = message.zone_status;

            isMessageReceived = true;
        }
        private void Update()
        {
            // TODO: Expand on this, and have so they're all hidden when it's toggled (will need it to trigger a function to reset all colour of zones
            if (isMessageReceived && ToggleStatus.IsToggled)
            {
                ProcessMessage();
            }
        }



        private void ProcessMessage()
        {   
            if (current_zone != null && current_block != null)
            {
                current_zone.GetComponent<MeshRenderer>().material = default_material;
                current_block.GetComponent<MeshRenderer>().material = default_material; 
            }

            // Find the objects
            for (int i = 0; i < highlightable_objects.Length; i++)
            {
                if (highlightable_objects[i].name.Equals(block_selected.ToString()))
                {
                    current_block = highlightable_objects[i];
                }
                else if (highlightable_objects[i].name.Equals(zone_selected.ToString()))
                {
                    current_zone = highlightable_objects[i];
                }
            }

            // Update their colour based on the message's content
           UpdateColour();

            // Play transition noise
            audio_source.PlayOneShot(transition_sound, 0.7F);

            isMessageReceived = false;
        }

        public void UpdateRobotSelection()
        {
            if (current_zone == null || current_block == null)
            {
                return;
            }
            if (!ToggleStatus.IsToggled)
            {
                // Reset previous block/zone to their default
                current_zone.GetComponent<MeshRenderer>().material = default_material;
                current_block.GetComponent<MeshRenderer>().material = default_material;   
            }
            else
            {
                UpdateColour();
            }
        }

        private void UpdateColour()
        {
             if (block_status == 1)
            {
                current_block.GetComponent<MeshRenderer>().material = red_highlight;
            }
            else if (block_status == 2)
            {
                current_block.GetComponent<MeshRenderer>().material = yellow_highlight;
            }
            if (zone_status == 1)
            {
                current_zone.GetComponent<MeshRenderer>().material = red_highlight;
            }
            else if (zone_status == 2)
            {
                current_zone.GetComponent<MeshRenderer>().material = yellow_highlight;
            }
        }
    }
}
