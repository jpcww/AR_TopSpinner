using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviourPunCallbacks       // to access 'Photon callbacks' 
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UI_LoginGameObject;

    [Header("Lobby UI")]
    public GameObject UI_LobbyGameObject;
    public GameObject UI_3DGameObject;

    [Header("Connection Status UI")]
    public GameObject UI_ConnectionSTatusGameObject;
    public Text connectionStatusText;                       // To use the text implemented in 'UI_connectionStatus'
    public bool showConnectionStatus = false;

    #region Unity Moethods
    // Start is called before the first frame update
    void Start()
    {
        // showing only the Lobby UI since already connected to Photon Server
        if (PhotonNetwork.IsConnected)
        {
            UI_LobbyGameObject.SetActive(true);
            UI_3DGameObject.SetActive(true);

            UI_ConnectionSTatusGameObject.SetActive(false);
            UI_LoginGameObject.SetActive(false);
        }

        // showing the login UI since never connected to Photon Server
        else
        {
            UI_LobbyGameObject.SetActive(false);
            UI_3DGameObject.SetActive(false);

            UI_ConnectionSTatusGameObject.SetActive(true);
            UI_LoginGameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        { connectionStatusText.text = "Connection Status : " + PhotonNetwork.NetworkClientState; }
    }
    #endregion


    #region UI Callbackk Methods

    public void OnEnterGameButtonClicked()
    {

        string playerName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(playerName))       // checking whether 'playerName' is empty
        {

            UI_LobbyGameObject.SetActive(false);
            UI_3DGameObject.SetActive(false);
            UI_LoginGameObject.SetActive(false);

            UI_ConnectionSTatusGameObject.SetActive(true);      // deciding which UI on, connection status UI is on since the button has been pressed

            if (!PhotonNetwork.IsConnected)          // chekcing whether it's conencted to 'photon' network, and if not....
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();   // Connecting the user to 'photon' network
            }
        }

        else
        {
            Debug.Log("Player name is invalid or empty");
        }

    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");               // 'SceneLoader' class inherited 'Singleton' class and 'LoadScene()' is a public method
    }


    #endregion


    #region Photon Callback methods         // to track the network connection status

    public override void OnConnected()      // it's called after 'Connect' button is pressed
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()  // called when the user is successfully connected to the server
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " "+ "is connected to Photon server");

        UI_ConnectionSTatusGameObject.SetActive(false);
        UI_LoginGameObject.SetActive(false);

        UI_LobbyGameObject.SetActive(true);     // deciding which UI on, Lobby and 3D UI are on since successfully connected to the server
        UI_3DGameObject.SetActive(true);
        showConnectionStatus = true;
    }



    #endregion
}
