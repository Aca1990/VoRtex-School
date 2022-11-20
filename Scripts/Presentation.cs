using System;
using System.IO;
using System.Net;
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
    public static bool PresentationCameraActive;

    public NetworkIdentity userIdentityWithAuthority;
    Uri baseUri;

    void Start()
    {
        slideNumber = 1;
        PresentationCameraActive = false;
        slideName = "slide1.png";
        imageCount = 38;//Directory.GetFiles(DBManager.microLesson.presentation_ppt_content, "*.png").Length;
        Debug.Log("Presentation loaded");

        // set first person camera
        firstPersonPresentation = gameObject.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonPresentation.name);
        firstPersonPresentationCamera = firstPersonPresentation.transform.GetChild(0).gameObject;
        Debug.Log(firstPersonPresentationCamera.name);
        firstPersonPresentationCamera.SetActive(false);

        //set up initial slide
        baseUri = new Uri(DBManager.microLesson.presentation_ppt_content);
        Uri myUri = new Uri(baseUri, slideName);
        path = myUri.ToString(); //Path.Combine(DBManager.microLesson.presentation_ppt_content, slideName);
        Debug.Log("presentation path " + path);
        //path = @"C:\MAMP\htdocs\presentations\acauser123\virtual_reality\slide1.png";
        Texture2D tex = LoadPNG(path);
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        firstPersonPresentation.GetComponent<Renderer>().material.mainTexture = tex;
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
        //path = Path.Combine(DBManager.microLesson.presentation_ppt_content, slideName);
        Uri myUri = new Uri(baseUri, slideName);
        path = myUri.ToString();
        Debug.Log("presentation path " + path);
        //path = @"C:\MAMP\htdocs\presentations\acauser123\virtual_reality\slide1.png";
        Texture2D tex = LoadPNG(path);
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        firstPersonPresentation.GetComponent<Renderer>().material.mainTexture = tex;
    }

    private Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;
        Debug.Log(filePath);
        //if (File.Exists(filePath))
        //{
        //    fileData = File.ReadAllBytes(filePath);
        //    tex = new Texture2D(2, 2);
        //    tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        //}
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
        return tex;
    }

    private byte[] GetBytesFromUrl(string url)
    {
        byte[] b;
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
        WebResponse myResp = myReq.GetResponse();

        Stream stream = myResp.GetResponseStream();
        //int i;
        using (BinaryReader br = new BinaryReader(stream))
        {
            //i = (int)(stream.Length);
            b = br.ReadBytes(500000);
            br.Close();
        }
        myResp.Close();
        return b;
    }
}