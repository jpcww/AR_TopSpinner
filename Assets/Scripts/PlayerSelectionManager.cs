using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    
    public Transform PlayerSwitcherTransform;       // the targer to be rotated


    public Button next_Button;
    public Button previous_Button;

    public int playerSelectionNumber;               // this will be matched to the array of the spinner top
    public GameObject[] spinnerTopMedels;



    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public GameObject UI_Selection;
    public GameObject UI_AfterSelection;


    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);

        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion


    #region UI Callback Methods

    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if (playerSelectionNumber >= spinnerTopMedels.Length)
            playerSelectionNumber = 0;
        Debug.Log(playerSelectionNumber);

        next_Button.enabled = false;
        previous_Button.enabled = false;
        StartCoroutine(Rotate(Vector3.up, PlayerSwitcherTransform, 90, 1.0f));

        // Change the UI by the spinners
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
            playerModelType_Text.text = "Attack";

        else
            playerModelType_Text.text = "Defend";
    }
    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;
        if (playerSelectionNumber < 0)
            playerSelectionNumber = spinnerTopMedels.Length - 1;
        Debug.Log(playerSelectionNumber);

        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, PlayerSwitcherTransform, -90, 1.0f));

        // Change the UI by the spinners
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
            playerModelType_Text.text = "Attack";

        else
            playerModelType_Text.text = "Defend";
    }

    public void onSelectButtonClicked()
    {
        UI_Selection.SetActive(false);
        UI_AfterSelection.SetActive(true);


        ExitGames.Client.Photon.Hashtable playerSelctionProp = new ExitGames.Client.Photon.Hashtable { { MultiPlayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelctionProp);                                  // how to set up custom properties to a player
        // Comprehension : create and save a custom player property in Hashtable class
        // Comprehension : by creating a class, which will be const, the property can be accessed throughout the project
        // Comprehension : by that, this property can be accessed in the player scene to load the proper spinner top model
    }

    public void OnReSelectButtonClicked()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {

        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation*Quaternion.Euler(axis*angle); // the way of rotating a vector by another vector

        float elapsedTime = 0.0f;
        while(elapsedTime < duration)         
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration); 
            elapsedTime += Time.deltaTime;
            yield return null;

            // Comprehension : Rotate the target axis by interpolate the value of axis between the 2 Quaternion values
            // Comprehension : by 'elapsedtime/duriation', which indicates the values in between until 'elapsedtime' reaches 'duration(1 sec)'
            // Comprehension : when the timer has reached 1 sec(when it's filled with 1 sec, I say), the loop ends 
        }

        transformToRotate.rotation = finalRotation;
        // Comprehension : due to 'while(elapsedTime < duration)', with the loop, the axis won't be able to reach the final roation, so set it manually
        // Comprehension : the reason why '=' is not put in the condition is that it cannot reach the exactly same value 


        next_Button.enabled = true;
        previous_Button.enabled = true;
    }

    #endregion
}
