using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public GameObject center;
    public float angularSpeed;
    public bool maintainDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(center.transform.position, Vector3.forward, angularSpeed * Time.deltaTime);
        if(maintainDirection) gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, -angularSpeed * Time.deltaTime);
    }
}
