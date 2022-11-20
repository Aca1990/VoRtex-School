using UnityEngine;

public class NPC : Interactable
{
    public override void CmdInteractF(GameObject obj)
    {
        Debug.Log("Interacting with NPC using F command");
    }

    public override void CmdInteractE(GameObject obj)
    {
        Debug.Log("Interacting with NPC using E command");
    }
}