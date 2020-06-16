using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    ARRaycastManager m_ARRayCastManager;
    static List<ARRaycastHit> raycast_Hits = new List<ARRaycastHit>();

    public Camera arCamera;
    public GameObject battleArenaGameObject;

    private void Awake()
    {
        m_ARRayCastManager = GetComponent<ARRaycastManager>();  // 'GetComponent<>()' : to access a componenet which is attached to the same gameObject
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Implementing Raycasting towards the center of the screen
        // AR RayCast Manager needs to be attached to ARSession Origin

        Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = arCamera.ScreenPointToRay(centerOfScreen); // Defining 'ray' that is cast from ARCamera to the center of the screen

        if(m_ARRayCastManager.Raycast(ray, raycast_Hits,TrackableType.PlaneWithinPolygon))
        {
            // Intersection with the detected plane
            Pose hitPose = raycast_Hits[0].pose;
            Vector3 positionToBePlaced = hitPose.position;

            battleArenaGameObject.transform.position = positionToBePlaced;

            // By doing so, it's able to place the battle area on the detected plane
        }


    }
}
