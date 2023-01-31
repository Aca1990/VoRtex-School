using UnityEngine.Networking;
using UnityEngine;
using TMPro;

public class SetupLocalPlayer : NetworkBehaviour {

	Animator animator;

	[SyncVar (hook = "OnChangeAnimation")]
	public string animState = "idle";

    private GameObject clickedObject;

    public GameObject username_text;

    void OnChangeAnimation (string aS)
    {
		if(isLocalPlayer) return;
		UpdateAnimationState(aS);
    }


	[Command]
	public void CmdChangeAnimState(string aS)
	{
		UpdateAnimationState(aS);
	}

	void UpdateAnimationState(string aS)
	{
		if(animState == aS) return;
		animState = aS;
		if(animState == "idle")
			animator.SetBool("Idling",true);
		else if (animState == "run")
			animator.SetBool("Idling",false);
	}

    public override void OnStartLocalPlayer()
    {
        CmdSendNameToServer();
    }

    // Use this for initialization
    void Start () 
	{
		animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idling", true);

		if(isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
			CameraFollow360.player = gameObject.transform;
        }
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}
	}

    [Client]
    void CmdSendNameToServer()
    {
        gameObject.name = DBManager.username;
        CmdUpdateName(DBManager.username); //PlayerPrefs.GetString("username")
        Debug.Log("Local user name added");
        username_text.SetActive(false);
    }

    [Command]
    public void CmdUpdateName(string name)
    {
        RpcUpdateName(name);
    }

    [ClientRpc]
    public void RpcUpdateName(string name)
    {
        username_text.GetComponent<TextMeshPro>().text = name;
        gameObject.name = name;
    }

    void Update()
    {
        if (isServer)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (clickedObject != hit.transform.gameObject)
                    { 
                        clickedObject = hit.transform.gameObject;

                        if(clickedObject.tag == "Users")
                        {
                            CmdUpdateUsersStatus();
                        }
                    }
                }
            }
        }
    }

    [Command]
    private void CmdUpdateUsersStatus()
    {
        Debug.Log(clickedObject.name);
        Debug.Log(clickedObject.tag);
        GameObject.Find("PresentationPlane(Clone)").GetComponent<Presentation>().userIdentityWithAuthority = clickedObject.GetComponent<NetworkIdentity>();
        //clickedObject.GetComponent<SetupLocalPlayer>().username_text.GetComponent<TextMeshPro>().color = new Color32(0, 255, 0, 255);
        RpcUpdateUserData(clickedObject, new Color32(0, 255, 0, 255));

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("Users"))
        {
            if (fooObj != clickedObject)
            {
                //fooObj.GetComponent<SetupLocalPlayer>().username_text.GetComponent<TextMeshPro>().color = new Color32(255, 255, 255, 255);
                RpcUpdateUserData(fooObj, new Color32(255, 255, 255, 255));
            }
            fooObj.GetComponent<SetupLocalPlayer>().RpcUpdateName(fooObj.name);
        }

        GameObject.Find("PresentationPlane(Clone)").GetComponent<Presentation>().CmdSetAuth(gameObject);
        //GameObject.Find("Automobile").GetComponent<InteractableModels>().CmdSetAuth(gameObject);
    }

    [ClientRpc]
    public void RpcUpdateUserData(GameObject fooObj, Color32 newColor)
    {
        fooObj.GetComponent<SetupLocalPlayer>().username_text.GetComponent<TextMeshPro>().color = newColor;
    }
}
