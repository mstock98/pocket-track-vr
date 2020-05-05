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
    [SerializeField] float distancePerStep = 1.0f;
    private PTVRStepReceiver _stepReceiver;
    private string _stepReceiverAddress;
    private GameObject _player;
    [SerializeField] XRNode _VRNode    = XRNode.Head;    
    
    public string GetStepReceiverAddress() {
        return _stepReceiverAddress;
    }

    void Start() {
        Debug.Log("[PTVR] Step in place script starting...");

        _stepReceiver = new PTVRStepReceiver();
        _stepReceiverAddress = _stepReceiver.GetIPAddressAndPort();

        if (((_player = GameObject.Find("OVRCameraRig")) == null)) {
            throw new Exception("[PTVR] Could not find OVRCameraRig");
        }
    }

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