using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;
    public Text InfoText;

    public void CallRegister()
    {       
        StartCoroutine(RegisterUser());
    }

    IEnumerator RegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        UnityWebRequest www = UnityWebRequest.Post($"http://{NetworkConstants.IpAddress}/sqlconnect/register.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            InfoText.text = www.error;
        }
        else
        {
            if (www.downloadHandler.text == "0")
            {
                Debug.Log("User created");
                InfoText.text = "User created";
            }
            else
            {
                Debug.Log("User creation failed. Error #" + www.downloadHandler.text);
                InfoText.text = "User creation failed. Error #" + www.downloadHandler.text;
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
