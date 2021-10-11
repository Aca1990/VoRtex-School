using System;
using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour
{
    public virtual void CmdSetAuth(GameObject obj)
    {
        Debug.Log("Set auth");
    }

    public virtual void CmdRemoveAuth(GameObject obj)
    {
        Debug.Log("Remove auth");
    }

    public virtual void CmdInteractF(GameObject obj)
    { 
        Debug.Log("Interacting with base class using F command");
    }

    public virtual void CmdInteractE(GameObject obj)
    {
        Debug.Log("Interacting with base class using E command");
    }

    public virtual void CmdInteractI(GameObject gameObject)
    {
        Debug.Log("Interacting with base class using I command");
    }
}