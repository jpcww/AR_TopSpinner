using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerSetup : MonoBehaviourPun         // To access 'PhotonView' component
{
    public TextMeshProUGUI playerTextReference;     // To access the UI for Player Name UI
    // Start is called before the first frame update
    void Start()
    {
       if(photonView.IsMine) // To tell the 'PhotonView' belongs to the local client(without this, one player can controller both Tops)
        {
            // the player is the local one
            transform.GetComponent<MovementController>().enabled = true;   // To access 'Movement Controller' script
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);   // Accessing the joystick component
        }

       else
        {
            // the player is the remote one
            transform.GetComponent<MovementController>().enabled = false;   // To access 'Movement Controller' script
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);   // Accessing the joystick component
        }

        SetPlayerName(); // to show the player name UI as soon as the player gameobject is instantiated
    }

    void SetPlayerName()
    {
        if(playerTextReference !=null)
        {
            if (photonView.IsMine)
            {
                playerTextReference.text = "YOU";
                playerTextReference.color = Color.red;
            }

            else
            {
                playerTextReference.text = photonView.Owner.NickName;
            }
        }
    }
}
