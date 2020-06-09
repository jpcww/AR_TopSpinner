using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 3600;
    public bool dospin = false;

    private Rigidbody rb;

    public GameObject playerGraphics; // to get the gameObject which belongs to the gameObject this script is attached to

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(dospin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0)); // Reason to use 'time.deltaTime' : 'Update()' is called every frame, which is not a fixed value. By using 'time.deltaTime', the change is set to happend every second

        }
    }
}
