using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView m_photonView;
    public Rigidbody rb; // added so we can turn the kinematic option on and off
    public bool isBeingHeld = false; // Added so we can keep track if an object is being held or is not being held. Start as not being held.

    string grabbedName;
    string objectName;
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();

        Debug.Log(m_photonView.gameObject.name + ":could be:" + gameObject.name);
        grabbedName = m_photonView.gameObject.name;
        objectName = gameObject.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Defines rb as this instance gameObject rigidbody component.
    }

    // Update is called once per frame
    void Update()
    {    
        if (!isBeingHeld) // If the obect is not being held then set the rigidbody kinematic to false
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        if (isBeingHeld)// if the object is being held then set the rigidbody kinematic to true
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }



    void TransferOwnership()
    {
        Debug.Log("TransferOwnership()");
        m_photonView.RequestOwnership();
    }


    public void OnSelectEnter()
    { 
        Debug.Log("OnSelectEnter()");
        Debug.Log("Grabbed");
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered, objectName);// !!!!!!!! Call right away and send the objectName so all the other RPC will be able to check if they need to add kinestetic or not.

        if (m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("Not requesting ownership. Already mine.");
        }
        else
        {
            TransferOwnership();
        }

    }



    public void OnSelectExit()
    {
        Debug.Log("OnSelectExit()");
        Debug.Log("Released");
        m_photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered, objectName);  //  Send the objectName so that all the RPC will only remove kinemtic if it is the correct object.
    }



    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != m_photonView)
        {
            return;
        }
        Debug.Log("OnOwnershipRequest()");
        Debug.Log("OnOwnerShip Requested for: " + targetView.name + " from " + requestingPlayer.NickName);

        m_photonView.TransferOwnership(requestingPlayer);
    }



    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("OnOwnershipTransfered()");      
    }



    [PunRPC]
    public void StartNetworkGrabbing(string whatIsGrabbed) // When grabbing begins change the bool to represent that the object is being held
    {
        Debug.Log("StartNetworkGrabbing()");
        if (whatIsGrabbed == objectName)
        {         
            isBeingHeld = true;
        }
    }



    [PunRPC]
    public void StopNetworkGrabbing(string whatIsGrabbed) // When the object is released change the bool to represent that the object is not being held
    {
        Debug.Log("StopNetworkGrabbing()");
        if (whatIsGrabbed == objectName)
        {
            Debug.Log("theGrabbedObjectView.gameObject.name == gameObject.name");
            isBeingHeld = false;
        }
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }
}