using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    public Text InfoText;

    public static bool loadScene;

    public GameObject multiPlayerMenu;

    public void CallLogin()
    {
        StartCoroutine(LoginUser());
    }

    IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        // NetworkConstants.IpAddress is db address
        string address = NetworkConstants.IpAddress; // "vortex-webplatform.com" or GetLocalIPAddress();
        string fullAddress = $"https://{address}/sqlconnect/login.php";
        UnityWebRequest www = UnityWebRequest.Post(fullAddress, form); // http://vortex-webplatform.great-site.net/sqlconnect/login.php
        www.SetRequestHeader("User-Agent", "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36");
        //www.SetRequestHeader("Access-Control-Allow-Origin", "*");
        //www.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        //www.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        //www.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            InfoText.text = www.error;
        }
        else
        {
            string[] serverData = www.downloadHandler.text.Split('\t');
            if (serverData[0] == "0")
            {
                Debug.Log("User logged in");
                InfoText.text = "User logged in";
                DBManager.username = nameField.text;
                DBManager.achievements = string.IsNullOrWhiteSpace(serverData[1]) ? new HashSet<string>() : new HashSet<string>(serverData[1].Split(',').ToList());
                DBManager.face_recognition_image_location = serverData[2];
                Debug.Log(serverData[3]);
                DBManager.role_id = Convert.ToInt32(serverData[3]);
                DBManager.id = Convert.ToInt32(serverData[4]);
                DBManager.microLesson.LessonName = serverData[5]; //"virtual_reality"
                DBManager.microLesson.LessonEnvironment = Convert.ToInt32(serverData[6]); //"virtual_reality"
                DBManager.microLesson.MicrolessonId = Convert.ToInt32(serverData[7]);
                //TODO: add presentation_ppt_content to DB?
                DBManager.microLesson.presentation_ppt_content = $"https://{address}/presentations/{DBManager.microLesson.LessonName}/"; // C:\MAMP\htdocs\presentations\acauser123\virtual_reality
                Debug.Log(@"C:\MAMP\htdocs\Python\face_recognize_webcam.py" + @" ..\" + DBManager.face_recognition_image_location);
                Debug.Log(DBManager.microLesson.presentation_ppt_content);

                // enable interactable and Presentation objecets
                DBManager.microLesson.PresentationON = Convert.ToBoolean(Convert.ToInt32(serverData[8]));
                DBManager.microLesson.InteractablesON = Convert.ToBoolean(Convert.ToInt32(serverData[9]));

                // set up avatar type model
                DBManager.avatar_type_id = serverData[10];

                MultiplayerMenu sn = multiPlayerMenu.GetComponent<MultiplayerMenu>();
                if (DBManager.role_id == 1) // teacher
                {
                    //load ip adress
                    NetworkConstants.ServerIpAddress = GetLocalIPAddress(); // GetPublicIp();//serverData[1];

                    form = new WWWForm();
                    form.AddField("user_id", DBManager.id);
                    form.AddField("user_ip", NetworkConstants.ServerIpAddress);
                    string post = $"https://{address}/sqlconnect/getIpAddress.php";
                    www = UnityWebRequest.Post(post, form);
                    www.SetRequestHeader("User-Agent", "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36");
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        InfoText.text = www.error;
                    }
                    else
                    {
                        serverData = www.downloadHandler.text.Split('\t');
                        if (serverData[0] == "0")
                        {
                            Debug.Log("IP address " + serverData[1]);
                            //yield return new WaitForSeconds(1);   //Wait 1 sec
                            //FaceRecognitionManager.Start();
                            sn.Host();
                            GoToVirtualRoom();
                        }
                    }
                }
                else if(DBManager.role_id == 2) // student
                {
                    form = new WWWForm();
                    form.AddField("microlesson_id", DBManager.microLesson.MicrolessonId);
                    string post = $"https://{address}/sqlconnect/getClassroomIp.php";
                    www = UnityWebRequest.Post(post, form);
                    www.SetRequestHeader("User-Agent", "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36");
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        InfoText.text = www.error;
                    }
                    else
                    {
                        serverData = www.downloadHandler.text.Split('\t');
                        if (serverData[0] == "0")
                        {
                            Debug.Log("IP address " + serverData[1]);
                            //yield return new WaitForSeconds(1);   //Wait 1 sec
                            //FaceRecognitionManager.Start();
                            NetworkConstants.ServerIpAddress = serverData[1];
                            sn.Connect();
                            GoToVirtualRoom();
                        }
                    }
                }

                //FaceRecognitionManager.Start();
                //Without face detection
                //GoToVirtualRoom();
            }
            else
            {
                Debug.Log("User logged in failed. Error #" + www.downloadHandler.text);
                InfoText.text = "User logged in failed. Error #" + www.downloadHandler.text;
            }
        }
    }

    private static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void VerifyLoginInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    private void GoToVirtualRoom()
    {
        Debug.Log("User logged in after face recognition");
        SceneManager.LoadScene(DBManager.microLesson.LessonEnvironment); // 1 classroom, 2 city, 3 park
    }

    private string GetPublicIp()
    {
        var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

        request.UserAgent = "curl"; // this will tell the server to return the information as if the request was made by the linux "curl" command

        string publicIPAddress;

        request.Method = "GET";
        using (WebResponse response = request.GetResponse())
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                publicIPAddress = reader.ReadToEnd();
            }
        }

        return publicIPAddress.Replace("\n", "");
    }

    void Update()
    {
        if (loadScene)
        {
            loadScene = false;
            GoToVirtualRoom();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
