using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviour
{
    public Spinner spinnerScript;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;

    public float common_Damage_Coefficient = 0.04f;

    public bool isAttacker;
    public bool isDefender;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient_Attacker = 10f;
    public float getDamaged_Coeeffiecient_Attacker = 1.2f;

    public float doDamage_Coefficient_Defender = 0.75f;
    public float getDamaged_Coeeffiecient_Defender = 0.2f;


    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if(gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }

        else if(gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;
            spinnerScript.spinSpeed = 4400;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Comparing the speeds of the SpinnerTops
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;    // 'colliison.collider.gameObject.' to access the other player's RigidBody

            Debug.Log("My speed: " + mySpeed + " -------- other player speed " + otherPlayerSpeed);

            if (mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You damaged other player");

                float defaultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * common_Damage_Coefficient;
                
                if (isAttacker)     // this verification is on the lcoal player, who is attacking
                {
                    defaultDamageAmount *= doDamage_Coefficient_Attacker;
                }

                else if (isDefender)
                {
                    defaultDamageAmount *= doDamage_Coefficient_Defender;
                }

                // until here, the damage amount is calculated based on the local player, who hits the remote player

                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    // Apply damage to the slower player, who is the remote one
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defaultDamageAmount);
                }
            }
        }
    }
    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (isAttacker)         // this verification is on the remote player, who gets attacked
        {
            _damageAmount *= getDamaged_Coeeffiecient_Attacker;
        }

        else if (isDefender)
        {
            _damageAmount *= getDamaged_Coeeffiecient_Defender;
        }

        spinnerScript.spinSpeed -= _damageAmount;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed; // ToString("F0") rounds the value to integers

    }
    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
