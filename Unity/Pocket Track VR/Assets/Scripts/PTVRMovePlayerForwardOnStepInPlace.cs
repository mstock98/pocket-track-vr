using UnityEngine;
using UnityEngine.XR;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PTVR;

/**
    Main PTVR Unity script
    Make sure to have this as a component of the OVRPlayerController
*/
public class PTVRMovePlayerForwardOnStepInPlace : MonoBehaviour {
    [SerializeField] float distancePerStep = 1.0f; // Distance (in meters) that the player will move with each registered step
    private PTVRStepReceiver _stepReceiver;
    private string _stepReceiverAddress;
    [SerializeField] int stepReceiverPort = 1422;
    private GameObject _player;
    [SerializeField] XRNode _VRNode    = XRNode.Head;    
    
    /**
        Gets the IPv4 address and port that the script is listening for step data on
        :return: address and port formated in a string as "#.#.#.#:<port number>"
    */
    public string GetStepReceiverAddress() {
        return _stepReceiverAddress;
    }

    /**
        Script setup. Initializes a PTVRStepReceiver and locates an OVRCameraRig.
    */
    void Start() {
        Debug.Log("[PTVR] Step in place script starting...");

        _stepReceiver = new PTVRStepReceiver(stepReceiverPort);
        _stepReceiverAddress = _stepReceiver.GetIPAddressAndPort();

        if (((_player = GameObject.Find("OVRCameraRig")) == null)) {
            throw new Exception("[PTVR] Could not find OVRCameraRig");
        }
    }

    /**
        Method that is called every frame to move the player according
        to how many steps were detected since last frame.
    */
    void Update() {
        int steps = 0;

        if ((steps = _stepReceiver.GetNumberOfStepsSinceLastCall()) != 0) {
            var quaternion  = InputTracking.GetLocalRotation(_VRNode);
            var euler = quaternion.eulerAngles;
            var yaw = euler.y;
            var adjustedYaw = 90.0f - yaw;

             var direction = new Vector3 (
                Mathf.Cos(adjustedYaw * Mathf.Deg2Rad),
                0f,
                Mathf.Sin(adjustedYaw * Mathf.Deg2Rad)
            );

            Debug.Log("[PTVR] Moving in direction: " + direction.x + " " + direction.y + " " + direction.z);

            _player.transform.position = _player.transform.position + (direction * distancePerStep * steps);
        }
    }
}