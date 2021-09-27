using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeCamera : NetworkBehaviour
{
    private GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;
    private int CamMode;
    public static bool CameraActive;

    private void Start()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Setup cammera");
            CamMode = 1;
            CameraActive = true;
            //FirstPersonCamera = gameObject.transform.Find("FirstPersonCamera").gameObject;
            ThirdPersonCamera = GameObject.Find("ThirdPersonCamera").gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (CameraActive && Input.GetButtonDown("Camera"))
            {
                if (CamMode == 1)
                {
                    CamMode = 0;
                }
                else
                {
                    CamMode = 1;
                }

                StartCoroutine(CameraChange());

            }
        }
    }

    private IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(0.01f);

        switch (CamMode)
        {
            case 0:
            {
                ThirdPersonCamera.SetActive(false);
                FirstPersonCamera.SetActive(true);
                break;
            }
            case 1:
            {
                ThirdPersonCamera.SetActive(true);
                FirstPersonCamera.SetActive(false);
                break;
            }
            default:
                break;
        }
    }
}
