using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Presentation : Interactable
{
    [SyncVar]
    private string path;
    [SyncVar]
    private int slideNumber;
    [SyncVar]
    private string slideName;
    [SyncVar]
    private int imageCount;

    private NetworkIdentity objNetId;

    private GameObject firstPersonPresentation;
    private GameObject firstPersonPresentationCamera;
    private Texture2D tex;
    private string savePath;
    private string saveFolder;

    public static bool PresentationCameraActive;
    public NetworkIdentity userIdentityWithAuthority;
    Uri baseUri;

    void Start()
    {
        slideNumber = 1;
        PresentationCameraActive = false;
        slideName = "slide1.png";
        Debug.Log("Presentation loaded");

        // set first person camera
        firstPersonPresentation = gameObject.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonPresentation.name);
        firstPersonPresentationCamera = firstPersonPresentation.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonPresentationCamera.name);
        firstPersonPresentationCamera.SetActive(false);

        //set up initial slide
        //path = @"C:\MAMP\htdocs\presentations\acauser123\virtual_reality\slide1.png";
        saveFolder = string.Format("{0}/{1}", Application.persistentDataPath, DBManager.microLesson.LessonName);
        savePath = string.Format("{0}/{1}", saveFolder, slideName);

        if (File.Exists(savePath))
        {
            SetUpImage();
        }
        else
        {
            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }

            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }

            StartCoroutine(UploadSlides());
        }
    }

    private void SetSlideName()
    {
        slideName = $"slide{slideNumber}.png";
    }

    [Command]
    public override void CmdSetAuth(GameObject obj)
    {
        if (isServer)
        {
            Debug.Log("From server");
            if(userIdentityWithAuthority != null)
            {
            //foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Users"))
            //{               
                //if (fooObj != obj)
                //{
                    //var oo = fooObj.GetComponent<NetworkIdentity>();
                    //if(userIdentityWithAuthority != oo)
                    //{
                objNetId = gameObject.GetComponent<NetworkIdentity>();        // get the object's network ID
                if (objNetId.clientAuthorityOwner != userIdentityWithAuthority.connectionToClient)
                {
                    if (objNetId.hasAuthority || objNetId.clientAuthorityOwner != null)
                    {
                        Debug.Log("Has auth");
                        objNetId.RemoveClientAuthority(objNetId.clientAuthorityOwner);
                    }
                    Debug.Log("No auth");
                        //userIdentityWithAuthority = oo;
                        //Debug.Log(GameObject.Find("PresentationPlane(Clone)").name);
                        //Debug.Log(fooObj.name);
                        objNetId.AssignClientAuthority(userIdentityWithAuthority.connectionToClient);
                        Debug.Log("Auth addeddddd");
                }
                //}
                //}
                //}   
            }
            // PlayerPref(Clone)
            //objNetId = GetComponent<NetworkIdentity>();        // get the object's network ID
            //Debug.Log(obj.name);
            //var oo = obj.GetComponent<NetworkIdentity>();
            //objNetId.AssignClientAuthority(oo.connectionToClient);
            //Debug.Log("Auth addeddddd");
        }
    }

    public override void CmdInteractI(GameObject gameObject)
    {
        //if(isLocalPlayer)
        //{
            if (PresentationCameraActive)
            {
                firstPersonPresentationCamera.SetActive(false);
            }
            else
            {
                firstPersonPresentationCamera.SetActive(true);
            }
            PresentationCameraActive = !PresentationCameraActive;
       // }
    }

    [Command]
    public override void CmdRemoveAuth(GameObject obj)
    {
        //objNetId = GetComponent<NetworkIdentity>();        // get the object's network ID
        //Debug.Log(obj.name);
        //var oo = obj.GetComponent<NetworkIdentity>();
        //objNetId.RemoveClientAuthority(oo.connectionToClient);
        //Debug.Log("Auth removed");
    }

    [Command]
    public override void CmdInteractF(GameObject obj)
    {
        Debug.Log("Interacting with Presentation using F command, go up");
        if (slideNumber < imageCount) // Cound png items in folder
        {
            if (isServer)
            {
                if (!DBManager.achievements.Contains("MoveObject"))
                {
                    var tokenContractService = GameObject.Find("[ManagerComponents]").GetComponent<TokenContractService>();
                    tokenContractService.SendFunds();
                    DBManager.achievements.Add("MoveObject");

                    UIManager canvas = GameObject.Find("Canvas").GetComponent(typeof(UIManager)) as UIManager;
                    canvas.addAchievements();
                }
                else
                {
                    var walletManager = GameObject.Find("[ManagerComponents]").GetComponent<WalletManager>();
                    walletManager.RefreshTopPanelView();
                }
                RpcLoadImage(true);
            }
            else
            {
                Debug.Log("No server");
                //objNetId = gameObject.GetComponent<NetworkIdentity>();
                //objNetId.AssignClientAuthority(connectionToClient);
                CmdLoadImage(true);
               // objNetId.RemoveClientAuthority(oo.connectionToClient);
            }
            //objNetId.RemoveClientAuthority(connectionToClient);
        }
    }

    [Command]
    public override void CmdInteractE(GameObject obj)
    {
        Debug.Log("Interacting with Presentation using E command, go back");
        if(slideNumber > 1)
        {
            if(isServer)
            {
                RpcLoadImage(false);
            }
            else
            {
                Debug.Log("No server");
                //objNetId = gameObject.GetComponent<NetworkIdentity>();
                //objNetId.AssignClientAuthority(connectionToClient);
                CmdLoadImage(false);
                //objNetId.RemoveClientAuthority(oo.connectionToClient);
            }

            //objNetId.RemoveClientAuthority(connectionToClient);
        }
    }

    [Command]
    private void CmdLoadImage(bool goUp)
    {
        RpcLoadImage(goUp);
    }

    [ClientRpc]
    private void RpcLoadImage(bool goUp)
    {
        if (goUp)
        {
            slideNumber++;
        }
        else
        {
            slideNumber--;
        }
        SetSlideName();
        saveFolder = string.Format("{0}/{1}", Application.persistentDataPath, DBManager.microLesson.LessonName);
        savePath = string.Format("{0}/{1}", saveFolder, slideName);

        if (File.Exists(savePath))
        {
            tex = LoadPNG(savePath);
        }
        else
        {
            tex = LoadPNG(path);
        }
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        firstPersonPresentation.GetComponent<Renderer>().material.mainTexture = tex;
    }

    private Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else
        {
            bool exist = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    exist = response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
            }
            if(exist)
            {
                Debug.Log("File exist");
                fileData = GetBytesFromUrl(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
            }
            else
            {
                Debug.Log("File do not exist");
            }
        }
        return tex;
    }

    private byte[] GetBytesFromUrl(string url)
    {
        byte[] b = null;
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
        myReq.Method = "GET";
        //myReq.Headers.Add("User-Agent", "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36");
        WebResponse response = myReq.GetResponse();

        if (response == null)
        {
            Debug.Log("No image");
        }
        else
        {
            Stream stream = response.GetResponseStream();
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();
            //b = downloadFullData(myReq);
        }

        return b;
    }

    byte[] downloadFullData(HttpWebRequest request)
    {
        using (WebResponse response = request.GetResponse())
        {

            if (response == null)
            {
                return null;
            }

            using (Stream input = response.GetResponseStream())
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while (input.CanRead && (read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
    IEnumerator UploadSlides()
    {
        WWWForm form = new WWWForm();
        form.AddField("microlesson_id", DBManager.microLesson.MicrolessonId);
        form.AddField("upload_images", saveFolder);
        string post = $"http://{NetworkConstants.IpAddress}/sqlconnect/downloadPresentation.php";
        UnityWebRequest www = UnityWebRequest.Post(post, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string[] serverData = www.downloadHandler.text.Split('\t');
            if (serverData[0] == "0")
            {
                SetUpImage();
            }
        }

        //using (UnityWebRequest www = UnityWebRequest.Get(url))
        //{
        //    www.SetRequestHeader("User-Agent", "Mozilla / 5.0(Windows NT 10.0; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 55.0.2883.87 Safari / 537.36");

        //    yield return www.SendWebRequest();
        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        Debug.Log(www.error);
        //    }
        //    else
        //    {
        //        File.WriteAllBytes(savePath, www.downloadHandler.data);
        //        tex = LoadPNG(savePath);
        //        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        //        firstPersonPresentation.GetComponent<Renderer>().material.mainTexture = tex;
        //    }
        //}
    }

    private void Ftp(string path)
    {
        try
        {
            // ftp://epiz_33020157@ftpupload.net/htdocs/presentations
            Uri serverUri = new Uri(path);
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return;
            }
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(path));
            reqFTP.Credentials = new NetworkCredential("epiz_33020157", "UdsrAv45dd");
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.Proxy = null;
            reqFTP.UsePassive = false;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream responseStream = response.GetResponseStream();
            FileStream writeStream = new FileStream(savePath, FileMode.Create);                

            int Length = 2048;
            Byte[] buffer = new Byte[Length];
            int bytesRead = responseStream.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = responseStream.Read(buffer, 0, Length);
            }
            writeStream.Close();
            response.Close();
        }
        catch (WebException wEx)
        {
            Debug.Log(wEx.Message);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void SetUpImage()
    {
        imageCount = Directory.GetFiles(saveFolder, "*.png").Length; // 38;
        tex = LoadPNG(savePath);
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        firstPersonPresentation.GetComponent<Renderer>().material.mainTexture = tex;
    }
}