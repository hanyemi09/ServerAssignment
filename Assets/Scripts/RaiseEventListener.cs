using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;

public class RaiseEventListener : MonoBehaviourPun
{

    public string msg;
    public string newmsg;
    public string evenNewMSG;
    public string USERID;
    public string bitcoinnum;
    public string ethnum;
    public string bnbnum;

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

        if(newmsg[0] == '\"' && newmsg[newmsg.Length - 1] == '\"')
        {
            evenNewMSG = newmsg.Trim(new char[] { '"' });
        }

        if(msg.Contains("bitcoin"))
        {
            int startIndex = 0;
            int endIndex = 0;
            USERID = bitcoinnum = ethnum = bnbnum = "";
            bool userid = false ,bitcoindone = false, ethereumdone = false, bnbdone = false;
            
            //[{"user_id":36,"bitcoin":0,"ethereum":0,"bnb":0}]
            for( int i = 0; i < msg.Length; i++)
            {
                if(msg[i] == ':')
                {
                    startIndex = i + 1;
                }
                else if(msg[i] == ',' || msg[i] == '}')
                {
                    endIndex = i - 1;
                    Debug.Log(startIndex + "," + endIndex);
                    if (!userid)
                    {
                        for (int j = 0; startIndex <= endIndex; j++)
                        {

                            USERID = USERID.Insert(j, msg[startIndex].ToString());
                            startIndex++;
                        }
                        userid = true;
                    }
                    else if (!bitcoindone)
                    {
                        for (int j = 0; startIndex <= endIndex; j++)
                        {

                            bitcoinnum = bitcoinnum.Insert(j, msg[startIndex].ToString());
                            startIndex++;
                        }
                        bitcoindone = true;
                    }
                    else if(!ethereumdone)
                    {
                        for (int j = 0; startIndex <= endIndex; j++)
                        {

                            ethnum = ethnum.Insert(j, msg[startIndex].ToString());
                            startIndex++;
                        }
                        ethereumdone = true;
                    }
                    else if(!bnbdone)
                    {
                        for (int j = 0; startIndex <= endIndex; j++)
                        {

                            bnbnum = bnbnum.Insert(j, msg[startIndex].ToString());
                            startIndex++;
                        }
                        bnbdone = true;
                    } 
                    
                }
            }
        }
        //Debug.Log("OnEvent : " + (int)obj.CustomData);
    }

   
}
