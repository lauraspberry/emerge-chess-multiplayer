using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    public RoomManagerihg roomManagerihg; 
    //public GameObject thePlayer;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered collider");
        roomManagerihg.OnEnterRoomButtonClicked_Outdoor();
    }

}
