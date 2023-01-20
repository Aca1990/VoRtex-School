using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
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
    private int achievementsCounter;
    // Only show it if needed.
    public static bool showPresenterInfo = false;
    public static bool showModelInfo = false;

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

            if (DBManager.microLesson.PresentationON)
            {
                achievementsCounter++;
            }
            if (DBManager.microLesson.InteractablesON)
            {
                achievementsCounter++;
            }
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
        string achievements  = string.Join(", ", achievementsArray);
        achiveDisplay.text = achievements;

        if (DBManager.achievements.Count >= achievementsCounter)
        {
            GenerateCertificate(achievements);
        }
    }

    private void GenerateCertificate(string achievements)
    {
        //Create a new PDF document.
        PdfDocument document = new PdfDocument();

        //Add a page to the document.
        PdfPage page = document.Pages.Add();

        //Create PDF graphics for the page.
        PdfGraphics graphics = page.Graphics;

        //Set the standard font.
        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

        //Add the text.
        graphics.DrawString($"Lesson {DBManager.microLesson.LessonName} completed", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 10));
        font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
        graphics.DrawString($"Bravo {DBManager.username}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 40));
        graphics.DrawString($"Your achievements are: {achievements}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 70));

        //Create the stream object.
        MemoryStream stream = new MemoryStream();

        //Save the document into memory stream.
        document.Save(stream);

        //If the position is not set to '0' then the PDF will be empty.
        stream.Position = 0;

        //Close the document.
        //C:/Users/YOUR_USER/AppData/LocalLow/DefaultCompany/VortexPrototype/lesson/user.pdf
        string saveFileLocation = string.Format("{0}/{1}/{2}Certificate.{3}", Application.persistentDataPath, DBManager.microLesson.LessonName, DBManager.username, "pdf");
        File.WriteAllBytes(saveFileLocation, stream.ToArray());
        // Create NFT token, send request
    
        System.Diagnostics.Process.Start(saveFileLocation, @"C:\Program Files(x86)\Microsoft\Edge\Application\msedge.exe"); // adobe: @"C:\Program Files\Adobe\Acrobat DC\Acrobat\Acrobat.exe"
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
        if (showPresenterInfo)
        {
            showModelInfo = false;
            windowRect = GUI.Window(0, windowRect, DialogWindowPresentation, "Commands info"); // TODO: move from onGUI
        }
        else if (showModelInfo)
        {
            showPresenterInfo = false;
            windowRect = GUI.Window(0, windowRect, DialogWindowModel, "Commands info");
        }
    }

    // This is the actual window.
    void DialogWindowPresentation(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(5, y, windowRect.width, 20), "Press Buttons F,E or I");

        if (GUI.Button(new Rect(5, y+20, windowRect.width - 10, 20), "OK"))
        {
            showPresenterInfo = false;
        }
    }

    void DialogWindowModel(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(5, y, windowRect.width, 20), "Press Button I");

        if (GUI.Button(new Rect(5, y + 20, windowRect.width - 10, 20), "OK"))
        {
            showModelInfo = false;
        }
    }
}
