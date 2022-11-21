using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text userDisplay;
    public Text achiveDisplay;

    private NetworkManagerHUDCustom hud;

    string path;
    public GameObject image;

    private Rect windowRect;
    // Only show it if needed.
    public static bool show = false;

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            userDisplay.text = DBManager.username;
            string[] achievementsArray = new string[DBManager.achievements.Count];
            DBManager.achievements.CopyTo(achievementsArray);
            achiveDisplay.text = string.Join(", ", achievementsArray);

            hud = FindObjectOfType<NetworkManagerHUDCustom>();
            if (hud != null)
            {
                hud.showGUI = true;
            }

            windowRect = new Rect(Screen.width - 160, Screen.height - 80, 150, 65);
        }
    }
    
    public void LogOut()
    {
        DBManager.LogOut();

        if (hud != null)
        {
            hud.showGUI = false;
        }

#if UNITY_EDITOR

        var networkObjects = FindObjectsOfType<NetworkManagerCustom>();
        foreach (var obj in networkObjects)
        {
            obj.StopHost();
            Debug.Log("Networking server disconnected");
        }

        var networkObjectsForge = FindObjectsOfType<BeardedManStudios.Forge.Networking.Unity.NetworkManager>();
        foreach (var obj in networkObjectsForge)
        {
            obj.Disconnect();
            Debug.Log("Chat server disconnected");
        }
#endif

        SceneManager.LoadScene(0);
    }

    public void LoadImage()
    {
        //path = @"C:\Users\acajo\source\repos\PPTtoImage\PPTtoImage\slide1.png";
       // path = EditorUtility.OpenFilePanel("Choose with new slide", "","png");
        //Texture2D tex = LoadPNG(path);
        //image.GetComponent<Renderer>().material.mainTexture = tex;
    }

    public void AddAchievements()
    {
        string[] achievementsArray = new string[DBManager.achievements.Count];
        DBManager.achievements.CopyTo(achievementsArray);
        achiveDisplay.text = string.Join(", ", achievementsArray);
    }

    private Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    void OnGUI()
    {
        if (show)
        {
            windowRect = GUI.Window(0, windowRect, DialogWindow, "Commands info"); // TODO: move from onGUI
        }
    }

    // This is the actual window.
    void DialogWindow(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(5, y, windowRect.width, 20), "Press Buttons F,E or I");

        if (GUI.Button(new Rect(5, y+20, windowRect.width - 10, 20), "OK"))
        {
            show = false;
        }
    }
}
