using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPosition;

    public GameObject battleArenaGameObject;

    public enum RaiseEventCodes // where to keep the event codes
    {
        PlayerSpawnEventCode = 0 // using 0 as an event code for spanwing player event
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Photon Callbacks methods
    public override void OnJoinedRoom()
    {
        //if(PhotonNetwork.IsConnectedAndReady)
        //{

        //}

        SpawnPlayer();
    }



    #endregion


    #region Private Methods

    private void SpawnPlayer()
    {
        object playerSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiPlayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            Debug.Log("Player selection number is " + (int)playerSelectionNumber);

            int randomSpawnPoint = Random.Range(0, spawnPosition.Length - 1);
            Vector3 instantiatePosition = spawnPosition[randomSpawnPoint].position;

            GameObject playerGameObject = Instantiate(playerPrefabs[(int)playerSelectionNumber],instantiatePosition, Quaternion.identity);

            PhotonView _photonView = playerGameObject.GetComponent<PhotonView>();

            if(PhotonNetwork.AllocateViewID(_photonView))
            {
                object[] data = new object[]
                {
                    playerGameObject.transform.position, battleArenaGameObject.transform, playerGameObject.transform.rotation, _photonView.ViewID, playerSelectionNumber
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true

                };


                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);
            }

            else
            {
                Debug.Log("Failed to allocate a viewID");
                Destroy(playerGameObject);
            }
        }
    }
    #endregion
}
