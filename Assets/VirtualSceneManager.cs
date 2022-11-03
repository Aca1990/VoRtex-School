using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class VirtualSceneManager : MonoBehaviour
{

    private void Awake()
    {
        if (DBManager.username == null)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void CallAddPlayerAchievement(string achievements)
    {
        StartCoroutine(SavePlayerAchievements(achievements));
    }

    IEnumerator SavePlayerAchievements(string achievements)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("achievements", achievements);

        UnityWebRequest www = UnityWebRequest.Post($"http://{NetworkConstants.IpAddress}/sqlconnect/savedata.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.downloadHandler.text == "0")
            {
                AddPlayerAchievement(achievements);
                Debug.Log("Data saved");
            }
            if(www.downloadHandler.text == "1")
            {
                Debug.Log("Achievements already added");
            }
            else
            {
                Debug.Log("Save failed. Error #" + www.downloadHandler.text);
            }
        }
    }

    private void AddPlayerAchievement(string achievements)
    {
        DBManager.achievements.Add(achievements);
    }

    public static VirtualSceneManager GetVirtualSceneManager()
    {
        GameObject go = GameObject.Find("VirtualScene");
        return (VirtualSceneManager)go.GetComponent(typeof(VirtualSceneManager)); ;
    }
}
