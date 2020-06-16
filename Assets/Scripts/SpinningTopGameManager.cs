using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class SpinningTopGameManager : MonoBehaviourPunCallbacks     // To use Photon's callback methods
{
    [Header("UI")]
    public GameObject UI_InformPanelGameObject;
    public TextMeshProUGUI UI_InformText;
    public GameObject searchForGameButtonGameObject;




    // Start is called before the first frame update
    void Start()
    {
        UI_InformPanelGameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Methods

    public void JoinRandomRoom()
    {
        UI_InformText.text = "Searching for available rooms.....";
        PhotonNetwork.JoinRandomRoom();
        searchForGameButtonGameObject.SetActive(false);
    }

    public void onQuickMatchbuttonClicked()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }


    public override void OnJoinedRoom()         // this method is called when the current user enter the room
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // when the current user creates the room, the player count starts with 1
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " Waiting for other players";
        }

        else
        {
            UI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " Waiting for other players";   // when the current player enters a exsiting room
            StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject, 2.0f));
        }
        Debug.Log(" joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  // this method is called when other player enter the room
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);

        UI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(UI_InformPanelGameObject, 2.0f)); // when 
    }

    #endregion

    #region Photon Callback Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        UI_InformText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + UnityEngine.Random.Range(0,1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        

        // Create the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }

}
