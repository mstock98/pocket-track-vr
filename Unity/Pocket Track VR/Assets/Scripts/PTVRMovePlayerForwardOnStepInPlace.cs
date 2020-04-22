using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        int steps;

        if ((steps = _stepReceiver.getNumberOfStepsSinceLastCall()) != 0)
        {
            Debug.Log("[PTVR] Steps collected from receiver: " + steps);
            // Thread.Sleep(0);
        }
    }
}