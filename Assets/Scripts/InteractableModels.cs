using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class InteractableModels : Presentation
{

    void Start()
    {
    }

    public override void CmdInteractI(GameObject gameObject)
    {
        Debug.Log("Interacting with 3D model");
        if (objNetId.hasAuthority || DBManager.role_id == 1)
        {
            Debug.Log("Play sound");
            GetComponent<AudioSource>().Play();
            if (!DBManager.achievements.Contains("InteractWithCar"))
            {
                var tokenContractService = GameObject.Find("[ManagerComponents]").GetComponent<TokenContractService>();
                tokenContractService.SendFunds();

                VirtualSceneManager.GetVirtualSceneManager().CallAddPlayerAchievement("InteractWithCar");
            }
            else
            {
                var walletManager = GameObject.Find("[ManagerComponents]").GetComponent<WalletManager>();
                walletManager.RefreshTopPanelView();
            }
            if (isServer)
            {
                RpcStart(true);
            }
            else
            {
                Debug.Log("No server");
                //objNetId = gameObject.GetComponent<NetworkIdentity>();
                //objNetId.AssignClientAuthority(connectionToClient);
                CmdStart(true);
                // objNetId.RemoveClientAuthority(oo.connectionToClient);
            }
            //objNetId.RemoveClientAuthority(connectionToClient);
        }
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
    private void CmdStart(bool goUp)
    {

    }

    [ClientRpc]
    private void RpcStart(bool goUp)
    {
       
    }
}