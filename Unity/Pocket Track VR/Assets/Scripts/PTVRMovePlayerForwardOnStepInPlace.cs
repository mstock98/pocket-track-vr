using UnityEngine;
// using UnityEngine.VR;
using UnityEngine.XR;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PTVR;

public class PTVRMovePlayerForwardOnStepInPlace : MonoBehaviour
{
    public readonly float DISTANCE_PER_STEP = 1.0f;
    private PTVRStepReceiver _stepReceiver;
    private GameObject _player;
    private GameObject _playerHead;
    [SerializeField] XRNode m_VRNode    = XRNode.Head;    
    
    void Start()
    {
        Debug.Log("[PTVR] Step in place script starting...");

        _stepReceiver = new PTVRStepReceiver();

        if (((_playerHead = GameObject.Find("ForwardDirection")) == null)) {
            Debug.Log("[PTVR] ERROR: Could not find ForwardDirection");
        }

        if (((_player = GameObject.Find("OVRCameraRig")) == null)) {
            Debug.Log("[PTVR] ERROR: Could not find OVRCameraRig");
        }
    }

    void Update()
    {
        int steps = 0;

        if ((steps = _stepReceiver.getNumberOfStepsSinceLastCall()) != 0)
        {
            var quaternion  = InputTracking.GetLocalRotation(m_VRNode);
            var euler = quaternion.eulerAngles;
            var yaw = euler.y;
            var adjustedYaw = 90.0f - yaw;

             var direction = new Vector3 (
                Mathf.Cos(adjustedYaw * Mathf.Deg2Rad),
                0f,
                Mathf.Sin(adjustedYaw * Mathf.Deg2Rad)
            );

            Debug.Log("[PTVR] Moving in direction: " + direction.x + " " + direction.y + " " + direction.z);

            _player.transform.position = _player.transform.position + (direction * DISTANCE_PER_STEP * steps);
        }
    }
}