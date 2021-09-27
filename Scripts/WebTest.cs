using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebTest : MonoBehaviour
{
    IEnumerator Start()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest request = UnityWebRequest.Get($"http://{NetworkConstants.IpAddress}/sqlconnect/webtest.php");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("This is server data: " + request.downloadHandler.text);
            string[] serverData = request.downloadHandler.text.Split('\t');

            foreach (var s in serverData)
            {
                Debug.Log(s);
            }
            //int.Parse(serverData[1]);
        }
    }
}
