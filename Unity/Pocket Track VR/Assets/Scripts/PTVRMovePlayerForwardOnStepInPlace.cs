using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PTVR;

public class PTVRMovePlayerForwardOnStepInPlace : MonoBehaviour
{
    private PTVRStepReceiver _stepReceiver;    
    
    void Start()
    {
        Debug.Log("[PTVR] Step in place script starting...");
        
        _stepReceiver = new PTVRStepReceiver();
    }

    void Update()
    {
        if (_stepReceiver.hasStepsToCollect())
        {
            int steps = _stepReceiver.getNumberOfStepsSinceLastCall();
            Debug.Log("[PTVR] Steps collected from receiver: " + steps);
        }
    }
}