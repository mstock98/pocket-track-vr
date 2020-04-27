using UnityEngine;
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

        // Based on https://answers.unity.com/questions/1380893/get-camera-rotation-constrained-to-leftright-ie-ya.html
        if ((steps = _stepReceiver.getNumberOfStepsSinceLastCall()) != 0)
        {
            // Debug.Log("[PTVR] Steps collected from receiver: " + steps);
            
            /* var yaw = _player.transform.rotation.eulerAngles.y;

            var direction = new Vector3 (
                Mathf.Cos(yaw * Mathf.Deg2Rad),
                0f,
                Mathf.Sin(yaw * Mathf.Deg2Rad)
            );

            _player.transform.position = _player.transform.position + (direction * DISTANCE_PER_STEP * steps); */

            var forward = new Vector3(_playerHead.transform.position.x, 0f, _playerHead.transform.position.z);
            forward.Normalize();

            Debug.Log("[PTVR] Moving in direction: " + forward.x + " " + forward.y + " " + forward.z);

            _player.transform.position = _player.transform.position + (forward * DISTANCE_PER_STEP * steps);
        }
    }
}