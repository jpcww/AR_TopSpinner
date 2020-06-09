using Photon.Pun;
using UnityEngine;

public class MySyncronization : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocty = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();             // To access the rigidbody component of the object this script is attached to
        photonView = GetComponent<PhotonView>();    // To access the photonview component of the object this script is attached to
        
        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();

    }

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.SerializationRate = 50; // to see the difference with different values
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()  // recommended when dealing with Physics (Rigidbody)
    {
        if (!photonView.IsMine) // for the gameObject of the remote player
        {
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance*(1.0f/PhotonNetwork.SerializationRate));  // By this, the gameObject will move from its original positon to the networked position by the amount of time for every physics update
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle*(1.0f/PhotonNetwork.SerializationRate));
        }

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)  // 'PhotonStream' class is a container class that sends or receives data from PhotonView
    {
        if(stream.IsWriting)    // if 'PhotonStream' is 'isWriting', the local player owns the PhotonView
        {
            // then PhotonView is owned by the local user, who controls this Gameobject
            // this one should send position, velocity and etc data to the gameObject of this player instantiated in the remote player's side
            stream.SendNext(rb.position);   // sending the position date to other players
            stream.SendNext(rb.rotation);   // sending the rotation date to other players

            if(synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if(synchronizeAngularVelocty)
            {
                stream.SendNext(rb.angularVelocity);
            }
            

        }

        else                   //  when 'PhotonView' is 'isReading'
        {
            // Called on my player gameobject that exists in remote player's game
            // Comprehension : this is for the gameObject, which is the same but copied on the remote player's side
            // Comprehension : then, it will read the data sent from the gameobject of this player instanteated in this player's side to syncronize

            networkedPosition = (Vector3)stream.ReceiveNext();  // Storing the streamed position data locally
            networkedRotation = (Quaternion)stream.ReceiveNext();   // Storing the streamed rotation data locally            

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkedPosition) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkedPosition;
                }
            }

            if (synchronizeAngularVelocty || synchronizeAngularVelocty)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)); //'PhotonNetwork.Time' : the server time, which is same for all the clients
                                                                                          // 'info.SnetServerTime' : the time taken to send the data to the server
                if(synchronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += rb.velocity * lag;     // Increase the velocity accuracy

                    distance = Vector3.Distance(rb.position, networkedPosition); // the distance between in the local and in the server
                }

                if(synchronizeAngularVelocty)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
        }
    }
}
