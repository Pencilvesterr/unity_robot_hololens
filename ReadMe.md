# Joint Action Framework using Augmented Reality and Eye Gaze
## Introduction
The repository contains the code used by Morgan Crouch for his Final Year Projects.

## Prerequisites 
* [Unity 2019.4 LTS](https://unity.com/releases/2019-lts)
* [Microsoft Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
* [Windows SDK 18362 or higher](https://developer.microsoft.com/en-us/windows/downloads/sdk-archive/)

## Unity HoloLens build instructions
1. Clone the repository.
2. Open the project inside Unity.
3. Open the Panda_Scene scene inside the Assets/Scenes folder.
4. Get the IP address of the ROS machine connecting with the robot. The static IP for the lab computer is ws://192.168.1.69:9090 
5. Modify the IP address in ROSConnector GameObject properties. Leave the port field untouched (9090)
6. Next, go to File -> Build Settings. 
7. Switch to Universal Windows Platform. Set Architecture to x86 if building for HoloLens 1, ARM64 if building for HoloLens 2.
8. Hit Build and ideally save to a 'Build' folder within the Unity repo. 
9. Head into the folder you saved the built solution to, and open the VS Solution.
10. Change the Solution Configuration to Release, Solution platform to x86 if building for HoloLens 1, ARM64 if building for HoloLens 2.
11. Set Device to 'Device' (Ensure Hololens is connected via USB to your Computer).
12. Ensure the Hololens is not asleep/turned off, and then click Build at the top, then Deploy <project name\>
13. Wait for the solution to build and it should be deployed and launched on your HoloLens when it's ready.
14. Run the ROS nodes from Morgan's 'ros_robot_controller' repo. This can be done within this repo using the bash command `./run_lab.sh`
