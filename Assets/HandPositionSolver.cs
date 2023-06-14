using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;

public class HandPositionSolver : MonoBehaviour
{
    [SerializeField] private ARHandTrackingManager handTrackingManager;
    [SerializeField] private Camera ARCam;
    [SerializeField] private float minConfidence = 0.85f;

    private Vector3 handPosition;
    public Vector3 HandPosition { get => handPosition; }

    // Start is called before the first frame update
    void Start()
    {
        handTrackingManager.HandTrackingUpdated += HandTrackingUpdated;
    }

    private void OnDestroy()
    {
        handTrackingManager.HandTrackingUpdated -= HandTrackingUpdated;
    }

    private void HandTrackingUpdated(HumanTrackingArgs args)
    {
        var detections = args.TrackingData?.AlignedDetections;
        if(detections == null)
        {
            return;
        }
        
        foreach(var detection in detections)
        {
            if(detection.Confidence < minConfidence)
            {
                return;
            }
            Vector3 detectionSize = new Vector3(detection.Rect.width, detection.Rect.height, 0);
            float depthEstimation = 2.0f + Mathf.Abs(1 - detectionSize.magnitude);
            handPosition = ARCam.ViewportToWorldPoint(new Vector3(detection.Rect.center.x, 1 - detection.Rect.center.y, depthEstimation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
