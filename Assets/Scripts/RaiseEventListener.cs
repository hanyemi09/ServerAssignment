using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;

public class RaiseEventListener : MonoBehaviourPun
{

    public string msg;
    public string newmsg;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnEvent(EventData obj)
    {
        Debug.Log("OnEvent : " + (string)obj.CustomData);
        msg = (string)obj.CustomData;
        if (msg.Contains(":" )&& msg.Contains( "}"))
        { 
            newmsg = msg.Split(':', '}')[1];
        }
        else
        {
            newmsg = null;
        }

        //Debug.Log("OnEvent : " + (int)obj.CustomData);
    }

   
}
