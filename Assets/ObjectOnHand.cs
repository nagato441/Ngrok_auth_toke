using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnHand : MonoBehaviour
{
    [SerializeField] private HandPositionSolver handPositionSolver;
    [SerializeField] private GameObject ARObject;
    [SerializeField] private float speedMovement = 0.5f;
    [SerializeField] private float speedRotation = 25.0f;

    private float minDistance = 0.05f;
    private float minAngleMagnitude = 2.0f;
    private bool shouldAdjustRotation;

    private void PlaceObj(Vector3 handPosition)
    {
        float distance = Vector3.Distance(handPosition, ARObject.transform.position);
        ARObject.transform.position = Vector3.MoveTowards(ARObject.transform.position, handPosition, speedMovement * Time.deltaTime);
        if(distance >= minDistance)
        {
            ARObject.transform.LookAt(handPosition);
            shouldAdjustRotation = true;
        } else
        {
            if(shouldAdjustRotation)
            {
                ARObject.transform.rotation = Quaternion.Slerp(ARObject.transform.rotation, Quaternion.identity, 2 * Time.deltaTime);
                Vector3 angles = ARObject.transform.rotation.eulerAngles;
                shouldAdjustRotation = angles.magnitude >= minAngleMagnitude;
            }
            else
            {
                ARObject.transform.Rotate(Vector3.up * speedRotation * Time.deltaTime);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlaceObj(handPositionSolver.HandPosition);
    }
}
