using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System;

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
    string loggedInEmail;
    int userid;

    public InputField CurrentName;
    public InputField DesiredName;

    public GameObject mainPage;
    public GameObject loginPage;

    public InputField DesiredPassword;
    public InputField confirmDesiredPassword;
    public InputField currentPassword;
    public Text passwordNotMatching1;
    public Text currentPassIncorrect;

    int minimumPassLength;
    int maximumPassLength;

    public Text passNoGood;
    public Text passNoGood2;
    public Text accountCreated;
    public Text accountCreationFailed;

    public InputField desiredEmail;
    public Text emailChangeTaken;
    public InputField currentEmail;

    public InputField deleteUser;
    public InputField deletePass;
    public InputField deleteEmail;
    public Text wronginfo;
    public GameObject deleteAccountPage;

    int bitcoin = 0;
    int ethereum = 0;
    int bnb = 0;

    public Text bitcoinnumber;
    public Text ethnumber;
    public Text bnbnumber;
    // Start is called before the first frame update
    void Start()
    {
        password.onEndEdit.AddListener(CheckPassword);
        confirmPassword.onEndEdit.AddListener(CheckPassword);
        DesiredPassword.onEndEdit.AddListener(CheckChangePassword);
        confirmDesiredPassword.onEndEdit.AddListener(CheckChangePassword);
        currentPassword.onEndEdit.AddListener(CheckCurrentPassword);
        minimumPassLength = 4;
        maximumPassLength = 20;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnSendMessageButtonClicked()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
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
            if (!string.IsNullOrEmpty(username.text) && !string.IsNullOrEmpty(password.text) && !string.IsNullOrEmpty(email.text) && !nameTaken.gameObject.activeSelf && !emailTaken.gameObject.activeSelf && password.text == confirmPassword.text && CheckPassBool(password,confirmPassword))
            {
                Debug.Log("Making accounts");
                string message = "INSERT INTO accounts (username, password,email) VALUES ( '"+ username.text.ToString() + "','" + password.text.ToString() + "','" + email.text.ToString() + "');";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(3, content, RaiseEventOptions.Default, SendOptions.SendReliable);

                StartCoroutine(AccountCreation());
                GetUserIDOnReg();
                StartCoroutine(InventoryCreation());
            }

            else
            {
                StartCoroutine(AccountCreationFailed());
                if (!CheckPassBool(password, confirmPassword))
                {
                }
            }
        }
    }

    public void GetUserIDOnReg()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("GETTHATNUMBER");
            string message = "SELECT * FROM accounts WHERE username = '" + username.text + "';";
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(6, content, RaiseEventOptions.Default, SendOptions.SendReliable);
            StartCoroutine(GetID());
        }

    }

    IEnumerator InventoryCreation()
    {
        //INSERT INTO inventory VALUES (1,1,2,3);
        yield return new WaitForSeconds(0.1f);
        string message = "INSERT INTO inventory VALUES (" + userid + ", 0,0,0 );";
        Debug.Log(message);
        byte[] content = Encoding.Default.GetBytes(message);
        Debug.Log(content);
        PhotonNetwork.RaiseEvent(3, content, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    IEnumerator AccountCreation()
    {
        accountCreated.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        accountCreated.gameObject.SetActive(false);
    }


    IEnumerator AccountCreationFailed()
    {
        accountCreationFailed.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        accountCreationFailed.gameObject.SetActive(false);
    }

    public void OnCheckUsernameAvailable(InputField ipf)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(ipf.text))
            {
                Debug.Log("Checking username");
                string message = " SELECT * FROM accounts WHERE username = '" + ipf.text.ToString() + "' ;";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
            }
        }
    }

    public void OnCheckUserNameStartCoroutine(Text textField)
    {
        StartCoroutine(JustWaitName(textField));
    }

    IEnumerator JustWaitName(Text text)
    {
        yield return new WaitForSeconds(0.1f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {

            text.gameObject.SetActive(true);
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            text.gameObject.SetActive(false);
        }
    }

    public void OnCheckEmailAvailable(InputField ipf)
    {
        if (PhotonNetwork.IsConnected)
        {
            bool hasAt = ipf.text.IndexOf('@') > 0;
            if (!string.IsNullOrEmpty(ipf.text.ToString()) && hasAt)
            {
                Debug.Log("Checking email");
                string message = " SELECT * FROM accounts WHERE email = '"  + ipf.text.ToString() + "' ;";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitEmail());

            }
        }
    }

    public void OnCheckEmailAvailableChange(InputField ipf)
    {
        if (PhotonNetwork.IsConnected)
        {
            bool hasAt = ipf.text.IndexOf('@') > 0;
            Debug.Log(ipf.text);
            if (!string.IsNullOrEmpty(ipf.text.ToString()) && hasAt)
            {
                Debug.Log("Checking email");
                string message = " SELECT * FROM accounts WHERE email = '" + ipf.text.ToString() + "' ;";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitEmailChange());

            }
        }
    }

    IEnumerator JustWaitEmailChange()
    {
        yield return new WaitForSeconds(0.1f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {
            emailChangeTaken.gameObject.SetActive(true);
            Debug.Log("Bro?");
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            Debug.Log("Wakey?");
            emailChangeTaken.gameObject.SetActive(false);
        }
    }

    IEnumerator JustWaitEmail()
    {
        yield return new WaitForSeconds(0.1f);
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
                string message = "SELECT * FROM accounts WHERE username = '" + usernameLogin.text.ToString() + "' AND password = '" + passwordLogin.text.ToString() + "';";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(JustWaitLogin());
            }
        }
    }
    public void ClearRegistrationInputFields()
    {
        username.text = "";
        password.text = "";
        confirmPassword.text = "";
        email.text = "";
    }

    public void ClearLoginFields()
    {
        usernameLogin.text = "";
        passwordLogin.text = "";
    }

    IEnumerator JustWaitLogin()
    {
        yield return new WaitForSeconds(0.1f);
        if (System.Convert.ToInt32(rel.newmsg) == 0 && !string.IsNullOrEmpty(rel.newmsg))
        {
            userorpassWrong.gameObject.SetActive(false);
            LoginUser();
        }
        else if (System.Convert.ToInt32(rel.newmsg) != 0 || string.IsNullOrEmpty(rel.newmsg))
        {
            userorpassWrong.gameObject.SetActive(true);
        }
        ClearLoginFields();
        
    }

    void LoginUser()
    {
        CurrentName.text = loggedInUsername = usernameLogin.text;
        loggedInPassword = passwordLogin.text;
        mainPage.gameObject.SetActive(true);
        loginPage.gameObject.SetActive(false);

        GetEmail();
        

        if (!string.IsNullOrEmpty(rel.newmsg))
        {

        }
    }

    public void GetUserID()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("GETTHATNUMBER");
            string message = "SELECT * FROM accounts WHERE username = '"+ loggedInUsername +"';";
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(6, content, RaiseEventOptions.Default, SendOptions.SendReliable);
            StartCoroutine(GetID());
        }

    }

    IEnumerator GetID()
    {
        yield return new WaitForSeconds(0.09f);
        if (!string.IsNullOrEmpty(rel.newmsg))
        {
            userid = Int32.Parse(rel.newmsg);
        }
        GetInventory();
    }

    public void GetEmail()
    {
        if (PhotonNetwork.IsConnected)
        {
            //  SELECT email FROM accounts WHERE username = 'ellysou';
            string message = "SELECT * FROM accounts WHERE username = '"+ loggedInUsername +"';";
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(5, content, RaiseEventOptions.Default, SendOptions.SendReliable);
            StartCoroutine(GetEmailC());
        }
    }

    IEnumerator GetEmailC()
    {
        yield return new WaitForSeconds(0.15f);
        if (!string.IsNullOrEmpty(rel.evenNewMSG))
        {
            currentEmail.text = loggedInEmail = rel.evenNewMSG;
        }
        GetUserID();
    }
    public void LogoutUser()
    {
        loggedInEmail = CurrentName.text = currentEmail.text = loggedInUsername = loggedInPassword = "";
        userid = -1;
    }

    public void ChangeUsername()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(DesiredName.text) && DesiredName.text != CurrentName.text)
            {
                Debug.Log("Hi");
                if (string.IsNullOrEmpty(rel.newmsg))
                {
                    //  UPDATE accounts SET username = 'abababa' WHERE username = 'ellysou';
                    string message = "UPDATE accounts SET username = '" + DesiredName.text.ToString() + "' WHERE username = '" + CurrentName.text.ToString() + "';";
                    Debug.Log(message);
                    byte[] content = Encoding.Default.GetBytes(message);
                    Debug.Log(content);
                    PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                    CurrentName.text = DesiredName.text;
                    DesiredName.text = "";
                }
            }
        }
    }

    public void CheckPassword(string text)
    {
        if (password.text != confirmPassword.text && password.text != null && confirmPassword.text != null)
        {
            passwordNotMatching.gameObject.SetActive(true);
        }
        else
        {
            passwordNotMatching.gameObject.SetActive(false);
            
        }
    }


    public bool CheckPassBool(InputField text1, InputField text2)
    {
        if (text1.text == text2.text && text1.text != null && text2.text != null)
        {
            if(text1.text.ToString().Length >= minimumPassLength && text1.text.ToString().Length <= maximumPassLength)
            {
                if(Regex.IsMatch(text1.text.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{4,20}$"))
                {
                    Debug.Log("OKAY");
                    return true;
                }

            }
        }
        else
        {
            Debug.Log("NOT OKAY");
            return false;
        }
        Debug.Log("NOT OKAY");
        return false;

    }

    public void CheckChangePassword(string text)
    {
        if (DesiredPassword.text != confirmDesiredPassword.text && !string.IsNullOrEmpty(DesiredPassword.text) && !string.IsNullOrEmpty(confirmDesiredPassword.text))
        {
            passwordNotMatching1.gameObject.SetActive(true);
        }
        else
        {
            passwordNotMatching1.gameObject.SetActive(false);
        }
    }

    public void CheckCurrentPassword(string text)
    {
        if (currentPassword.text != loggedInPassword)
        {
            currentPassIncorrect.gameObject.SetActive(true);
        }
        else
        {
            currentPassIncorrect.gameObject.SetActive(false);
        }
    }

    public void ChangePassword()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(DesiredPassword.text) && CheckPassBool(DesiredPassword, confirmDesiredPassword) && currentPassword.text.ToString() == loggedInPassword)
            {
                //  UPDATE accounts SET password = 'abababa' WHERE username = 'ellysou';
                string message = "UPDATE accounts SET password = '" + DesiredPassword.text.ToString() + "' WHERE username = '" + loggedInUsername + "';";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                loggedInPassword = DesiredPassword.text.ToString();
                DesiredPassword.text = confirmDesiredPassword.text = "";
            }
        }
    }

    public void ChangeEmail()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!string.IsNullOrEmpty(desiredEmail.text) && !emailChangeTaken.gameObject.activeSelf)
            {

                //  UPDATE accounts SET password = 'abababa' WHERE username = 'ellysou';
                string message = "UPDATE accounts SET email = '" + desiredEmail.text.ToString() + "' WHERE username = '" + loggedInUsername + "';";
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);
                StartCoroutine(ChangeDemEmail());

            }
        }
    }

    public void DeleteAccount()
    {
        if(PhotonNetwork.IsConnected)
        {
            if(deleteEmail.text == loggedInEmail && deletePass.text == loggedInPassword && deleteUser.text == loggedInUsername)
            {

                //  DELETE FROM inventory WHERE user_id = 1;
                string message = " DELETE FROM inventory WHERE user_id = " + userid;
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

                // DELETE FROM accounts WHERE user_id = 1;
                string message1 = " DELETE FROM accounts WHERE user_id = " + userid;
                Debug.Log(message1);
                byte[] content2 = Encoding.Default.GetBytes(message1);
                Debug.Log(content2);
                PhotonNetwork.RaiseEvent(4, content2, RaiseEventOptions.Default, SendOptions.SendReliable);
                wronginfo.gameObject.SetActive(false);
                deleteAccountPage.gameObject.SetActive(false);
                loginPage.gameObject.SetActive(true);
                LogoutUser();
            }
            else
            {
                wronginfo.gameObject.SetActive(true);
            }
        }

    }

    public void BuyBitcoin()
    {
        if (PhotonNetwork.IsConnected)
        {
            int number = Int32.Parse(rel.bitcoinnum) + 1;
            //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
            string message = " UPDATE inventory SET bitcoin = " + number.ToString() + " WHERE user_id = " + userid.ToString();
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

            GetInventory();
        }
    }

    public void SellBitcoin()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(Int32.Parse(rel.bitcoinnum) > 0)
            {

                int number = Int32.Parse(rel.bitcoinnum) - 1;
                //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
                string message = " UPDATE inventory SET bitcoin = " + number.ToString() + " WHERE user_id = " + userid.ToString();
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

                GetInventory();
            }

        }
    }

    public void BuyEthereum()
    {
        if (PhotonNetwork.IsConnected)
        { 

            int number = Int32.Parse(rel.ethnum) + 1;
            //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
            string message = " UPDATE inventory SET ethereum = " + number.ToString() + " WHERE user_id = " + userid.ToString();
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

            GetInventory();
        }
    }

    public void SellEthereum()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (Int32.Parse(rel.ethnum) > 0)
            {
                int number = Int32.Parse(rel.ethnum) - 1;
                //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
                string message = " UPDATE inventory SET ethereum = " + number.ToString() + " WHERE user_id = " + userid.ToString();
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

                GetInventory();
            }
        }
    }

    public void BuyBNB()
    {
        if (PhotonNetwork.IsConnected)
        {
            int number = Int32.Parse(rel.bnbnum) + 1;
            //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
            string message = " UPDATE inventory SET bnb = " + number.ToString() + " WHERE user_id = " + userid.ToString();
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

            GetInventory();
        }
    }

    public void SellBNB()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (Int32.Parse(rel.bnbnum) > 0)
            {
                int number = Int32.Parse(rel.bnbnum) - 1;
                //  UPDATE inventory SET bitcoin = number WHERE user_id = user_id ;
                string message = " UPDATE inventory SET bnb = " + number.ToString() + " WHERE user_id = " + userid.ToString();
                Debug.Log(message);
                byte[] content = Encoding.Default.GetBytes(message);
                Debug.Log(content);
                PhotonNetwork.RaiseEvent(4, content, RaiseEventOptions.Default, SendOptions.SendReliable);

                GetInventory();
            }
        }
    }

    public void GetInventory()
    {
        if (PhotonNetwork.IsConnected)
        {
            //  DELETE FROM inventory WHERE user_id = 1;
            string message = "SELECT * FROM inventory WHERE user_id = " + userid + ";";
            Debug.Log(message);
            byte[] content = Encoding.Default.GetBytes(message);
            Debug.Log(content);
            PhotonNetwork.RaiseEvent(7, content, RaiseEventOptions.Default, SendOptions.SendReliable);
            StartCoroutine(InvNumbers());
        }
    }

    IEnumerator InvNumbers()
    {
        yield return new WaitForSeconds(0.2f);
        bitcoinnumber.text = rel.bitcoinnum;
        ethnumber.text = rel.ethnum;
        bnbnumber.text = rel.bnbnum;
    }

    IEnumerator ChangeDemEmail()
    {
        yield return new WaitForSeconds(0.05f);
        loggedInEmail = currentEmail.text = desiredEmail.text;
        desiredEmail.text = "";
    }
}
