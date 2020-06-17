using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    ARPlacementManager m_ARPlacementManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public TextMeshProUGUI informUIPanel_Text;

    public GameObject scaleSlider;

    private void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_ARPlacementManager = GetComponent<ARPlacementManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        scaleSlider.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);


        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = false;
        m_ARPlacementManager.enabled = false;

        scaleSlider.SetActive(false);

        SetAllPlaneActiveOrDeactive(false);
        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);

        informUIPanel_Text.text = "Great! You place the Arena.. Now, Search for games to Battle!";
    }

    public void EnableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = true;
        m_ARPlacementManager.enabled = true;

        scaleSlider.SetActive(true);

        SetAllPlaneActiveOrDeactive(true);
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);

        informUIPanel_Text.text = "Move phone to detect planes and place the Battle Arena";
    }

    private void SetAllPlaneActiveOrDeactive(bool value)
    {
        foreach(var plane in m_ARPlaneManager.trackables)   // by doing so, it's possible to access the detected plane
        {
            plane.gameObject.SetActive(value);
        }
    }
}
