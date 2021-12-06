using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class SendMessage : MonoBehaviour
{
    public RaiseEventListener rel;
    public InputField username;
    public InputField password;
    public InputField confirmPassword;
    public InputField email;
    public Text passwordNotMatching;
    public Text nameTaken;
    public Text emailTaken;
    public Text validEmail;
    public InputField usernameLogin;
    public InputField passwordLogin;
    public Text userorpassWrong;

    string loggedInUsername;
    string loggedInPassword;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(password.text != confirmPassword.text && password.text != null && confirmPassword.text != null)
        {
            passwordNotMatching.gameObject.SetActive(true);
        }
        else
        {
            passwordNotMatching.gameObject.SetActive(false);
        }

        

        //if(rel.msg.Contains(username.text))
        //{
        //    nameTaken.gameObject.SetActive(true);
        //}
        //else
        //{
        //    nameTaken.gameObject.SetActive(false);
        //}

        //if (rel.msg.Contains(email.text))
        //{
        //    nameTaken.gameObject.SetActive(true);
        //}
        //else
        //{
        //    nameTaken.gameObject.SetActive(false);
        //}
    }

    public void OnSendMessageButtonClicked()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Sending Hello World");
            string message = "Hello World";
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(1, content, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void OnPrintTable()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Getting accounts");
            string message = "Hello World";
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(2, content, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void OnRegistration()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(username.text) && !string.IsNullOrEmpty(password.text) && !string.IsNullOrEmpty(email.text) && !nameTaken.gameObject.activeSelf && !emailTaken.gameObject.activeSelf && password.text == confirmPassword.text)
            {
                Debug.Log("Making accounts");
                string message = "'" + username.text.ToString() + "','" + password.text.ToString() + "','" + email.text.ToString() + "' ";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(3, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                
            }
        }
    }

    public void OnCheckUsernameAvailable()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(username.text))
            {
                Debug.Log("Checking username");
                string message =  username.text.ToString() ;
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitName()); 
            }
        }
    }

    IEnumerator JustWaitName()
    {
        yield return new WaitForSeconds(0.5f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {
            nameTaken.gameObject.SetActive(true);
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            nameTaken.gameObject.SetActive(false);
        }
    } 

    public void OnCheckEmailAvailable()
    {
        if (PhotonNetwork.IsConnected)
        {
            bool hasAt = email.text.IndexOf('@') > 0;
            if (!string.IsNullOrEmpty(email.text.ToString()) && hasAt)
            {
                Debug.Log("Checking email");
                string message = email.text.ToString();
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content); 
                PhotonNetwork.RaiseEvent(5, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitEmail());

            }
            if(!hasAt)
            {
               
               // validEmail.gameObject.SetActive(true);
            }
            else
            {
                // validEmail.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator JustWaitEmail()
    {
        yield return new WaitForSeconds(0.5f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {
            emailTaken.gameObject.SetActive(true);
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            emailTaken.gameObject.SetActive(false);
        }
    }

    public void OnCheckUserLoginCredentials()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(usernameLogin.text) && !string.IsNullOrEmpty(passwordLogin.text))
            {
                Debug.Log("Checking username and password login");
                string message = "username = '" + usernameLogin.text.ToString() + "' AND password = '" + passwordLogin.text.ToString() + "'";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(6, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitLogin());
            }
        }
    }

    IEnumerator JustWaitLogin()
    {
        yield return new WaitForSeconds(0.5f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {
            userorpassWrong.gameObject.SetActive(false);
            LoginUser();
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            userorpassWrong.gameObject.SetActive(true);
        }
    }

    void LoginUser()
    {
        loggedInUsername = usernameLogin.text;
        loggedInPassword = passwordLogin.text;
    }

    public void LogoutUser()
    {
        loggedInUsername = "";
        loggedInPassword = "";
    }
}
