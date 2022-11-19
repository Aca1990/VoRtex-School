using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public static class FaceRecognitionManager
{
    // 1. Declare Variables
    static Thread receiveThread; //1
    static UdpClient client; //2
    static int port; //3
    static bool checkAccess; //4

    static Process myProcess; //5
    public static void Start()
    {
        // call python script
        // Create new process start info 
        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("python.exe");
        // make sure we can read the output from stdout 
        myProcessStartInfo.UseShellExecute = false;
        myProcessStartInfo.RedirectStandardOutput = true;
        myProcessStartInfo.Arguments = $"http://{NetworkConstants.IpAddress}/Python/face_recognize_webcam.py" + @" ..\" + DBManager.face_recognition_image_location;

        myProcess = new Process();
        // assign start information to the process 
        myProcess.StartInfo = myProcessStartInfo;

        // start the process 
        myProcess.Start();

        // Read the standard output of the app we called.  
        // in order to avoid deadlock we will read output first 
        // and then wait for process terminate: 
        //StreamReader myStreamReader = myProcess.StandardOutput;
        //string myString = myStreamReader.ReadLine();

        // wait exit signal from the app we called and then close it. 
        //myProcess.WaitForExit();
        //myProcess.Close();

        // setup socket
        port = 5065; //1
        InitUDP();
        checkAccess = true;
    }

    private static void InitUDP()
    {
        UnityEngine.Debug.Log("UDP Initialized");

        receiveThread = new Thread(new ThreadStart(ReceiveData)); //1 
        receiveThread.IsBackground = true; //2
        receiveThread.Start(); //3
    }

    private static void ReceiveData()
    {
        client = new UdpClient(port); //1
        while (checkAccess) //2
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); //3
                byte[] data = client.Receive(ref anyIP); //4

                string text = Encoding.UTF8.GetString(data); //5
                UnityEngine.Debug.Log(text);

                myProcess.Kill();
                if (text == "Access")
                {
                    Login.loadScene = true;
                }
                client.Close();
                receiveThread.Abort();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.ToString()); //7
            }
        }
    }
}
