using UnityEngine;
using UnityEngine.Networking;

public class WorldInteraction : NetworkBehaviour
{
    private bool interactionReady;
    private GameObject interactedObject;

    private ChangeCamera changeCamera;

    // Start is called before the first frame update
    void Start()
    {
        interactionReady = false;
        interactedObject = null;
        //ChangeCamera.FirstPersonCamera = gameObject.transform.Find("FirstPersonCamera").gameObject;

        //if (!isServer)
        //    return;
        //UIManager canvas = GameObject.Find("Canvas").GetComponent(typeof(UIManager)) as UIManager;
        //if (canvas != null)
        //{
        //    Debug.Log("Object renamed to " + canvas.userDisplay.text);
        //    gameObject.name = canvas.userDisplay.text;
        //}
        // find change camera
        changeCamera = gameObject.GetComponent<ChangeCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactionReady)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetFInteraction(interactedObject.name);
            } else if (Input.GetKeyDown(KeyCode.E))
            {
                GetEInteraction(interactedObject.name);
            } else if (Input.GetKeyDown(KeyCode.I))
            {
                GetIInteraction(interactedObject.name);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isLocalPlayer)
        {
            Debug.Log("OnCollisionEnter");
            interactionReady = true;
            interactedObject = other.gameObject;

            if (interactedObject.tag == "Interactable")
            {
                UIManager.show = true;
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (isLocalPlayer)
        {
            Debug.Log("OnCollisionExit");
            interactionReady = false;
            interactedObject = null;
            UIManager.show = false;
        }
    }

    void GetFInteraction(string interaction)
    {
        switch (interaction)
        {
            case "PresentationPlane":
            case "PresentationPlane(Clone)":
            case "VideoPlane":
            {
                 //interactedObject.GetComponent<Interactable>().CmdSetAuth(gameObject);                 
                 interactedObject.GetComponent<Interactable>().CmdInteractF(gameObject);
                 //interactedObject.GetComponent<Interactable>().CmdRemoveAuth(gameObject);
                 break;
            }
            default:
            {
                Debug.Log("Non interactive object");
                break;
            }
        }
        //Example with ray
        //Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit interactionInfo;
        //if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        //{
        //GameObject interactedObject = interactionInfo.collider.gameObject;
        //if (interactedObject.name == "Presentation")
        //{
        //    interactedObject.GetComponent<Interactable>().Interact();
        //}
        //}
    }

    void GetEInteraction(string interaction)
    {
        switch (interaction)
        {
            case "PresentationPlane":
            case "PresentationPlane(Clone)":
            case "VideoPlane":
            {
                //interactedObject.GetComponent<Interactable>().CmdSetAuth(gameObject);
                interactedObject.GetComponent<Interactable>().CmdInteractE(gameObject);
                //interactedObject.GetComponent<Interactable>().CmdRemoveAuth(gameObject);
                break;
            }
            default:
            {
                Debug.Log("Non interactive object");
                break;
            }
        }
    }

    void GetIInteraction(string interaction)
    {
        switch (interaction)
        {
            case "PresentationPlane":
            case "PresentationPlane(Clone)":
            case "VideoPlane":
                {
                    interactedObject.GetComponent<Interactable>().CmdInteractI(gameObject);
                    break;
                }
            default:
                {
                    Debug.Log("Non interactive object");
                    break;
                }
        }
    }

}