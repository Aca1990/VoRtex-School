using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.Generic;

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

        UnityWebRequest www = UnityWebRequest.Post($"http://{NetworkConstants.IpAddress}/sqlconnect/login.php", form);
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
                DBManager.achievements = new HashSet<string>(serverData[1].Split(',').ToList());
                DBManager.face_recognition_image_location = serverData[2];
                Debug.Log(serverData[3]);
                DBManager.role_id = System.Convert.ToInt32(serverData[3]);
                DBManager.id = System.Convert.ToInt32(serverData[4]);
                // TODO: microlesson setup
                DBManager.microLesson.LessonName = "virtual_reality";
                //TODO: add presentation_ppt_content to DB
                DBManager.microLesson.presentation_ppt_content = $"http://{NetworkConstants.IpAddress}/presentations/{DBManager.microLesson.LessonName}/"; // C:\MAMP\htdocs\presentations\acauser123\virtual_reality
                Debug.Log(@"C:\MAMP\htdocs\Python\face_recognize_webcam.py" + @" ..\" + DBManager.face_recognition_image_location); 
                Debug.Log(DBManager.microLesson.presentation_ppt_content);
                //Without face detection -> GoToVirtualRoom();
                MultiplayerMenu sn = multiPlayerMenu.GetComponent<MultiplayerMenu>();
                if (DBManager.role_id == 1)
                {
                    sn.Host();
                }
                else if(DBManager.role_id == 2)
                {
                    sn.Connect();
                }


                // load ip adress
                form = new WWWForm();
                form.AddField("user_id", DBManager.id);
                www = UnityWebRequest.Post($"http://{NetworkConstants.IpAddress}/sqlconnect/getIpAddress.php", form);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    InfoText.text = www.error;
                }
                else
                {
                    Debug.Log("IP address " + www.downloadHandler.text); // TODO: ip address for networking, NetworkConstants.IpAddress is db address
                    //yield return new WaitForSeconds(1);   //Wait
                    //FaceRecognitionManager.Start();
                    GoToVirtualRoom();
                }
            }
            else
            {
                Debug.Log("User logged in failed. Error #" + www.downloadHandler.text);
                InfoText.text = "User logged in failed. Error #" + www.downloadHandler.text;
            }
        }
    }

    public void VerifyLoginInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }

    private void GoToVirtualRoom()
    {
        Debug.Log("User logged in after face recognition");
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if (loadScene)
        {
            loadScene = false;
            GoToVirtualRoom();
        }
    }
}
