using UnityEngine;

public class PTVRTestScript : MonoBehaviour
{
    private float update;

    void Awake()
    {
        Debug.Log("Awake");
        update = 0.0f;                                           
    }

    void Start()
    {
        Debug.Log("Start");
    }

    void Update()
    {
        update += Time.deltaTime;
        if (update > 1.0f) 
        {
            update = 0.0f;
            Debug.Log("Update");
        }
    }
}