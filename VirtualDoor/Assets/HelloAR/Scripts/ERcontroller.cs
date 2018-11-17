using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ERcontroller : MonoBehaviour {
    //we will fill this list with the planes that ARcore detected in the current frame
    private List<TrackedPlane> m_newTrackedPlanes = new List<TrackedPlane>();
    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        //check arcore session status
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        //we will fill our list with newly tracked planes in this sesssion of ARCore
        Session.GetTrackables<TrackedPlane>(m_newTrackedPlanes, TrackableQueryFilter.New);
        for (int i = 0; i < m_newTrackedPlanes.Count; ++i)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);
            grid.GetComponent<GridVisualizer>().Initialize(m_newTrackedPlanes[i]);
        }
        //check if user touched screen
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        //checking if user touched any of the tracked planes
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon,out hit))
        {
            //place portal over x,y
            Portal.SetActive(true);
            //create an anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //Face the camera
            Vector3 cameraPosition = ARCamera.transform.position;
            cameraPosition.y = hit.Pose.position.y;
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);
            Portal.transform.parent = anchor.transform;


        }
	}
}
