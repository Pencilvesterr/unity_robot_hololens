using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Visualization;
using RosSharp.RosBridgeClient.MessageTypes.Moveit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Written by Morgan Crouch 2020
 * Aims to update the zone and block selection of the robot in AR.
 */
namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(AudioSource))]
    public class BlockZoneSelectionSubsriber : UnitySubscriber<MessageTypes.Moveit.TrafficLight>
    {
        AudioSource audio_source;

        public Material default_material;
        public Material yellow_highlight;
        public Material red_highlight;
        public AudioClip transition_sound;

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
            isMessageReceived = false;
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
            if (isMessageReceived)
                ProcessMessage();
        }
        void ProcessMessage()
        {
            // Reset blocks to their default
            current_zone.GetComponent<MeshRenderer>().material = default_material;
            current_block.GetComponent<MeshRenderer>().material = default_material;


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

            // Play transition noise
            audio_source.PlayOneShot(transition_sound, 0.7F);

            isMessageReceived = false;
        }
    }
}
