using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerCustom : NetworkManager
{
    private bool fromServer = false;
    public GameObject treePrefab;

    public class MsgTypes
    {
        public const short PlayerPrefab = MsgType.Highest + 1;

        public class PlayerPrefabMsg : MessageBase
        {
            public short controllerID;
            public short prefabIndex;
        }
    }

    // in the Network Manager component, you must put your player prefabs 
    // in the Spawn Info -> Registered Spawnable Prefabs section 
    public short playerPrefabIndex;


    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");
        NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
        fromServer = true;
        base.OnStartServer();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnServerConnect");
        base.OnServerConnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect");
        client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
        base.OnClientConnect(conn);
    }

    private void OnRequestPrefab(NetworkMessage netMsg)
    {
        Debug.Log("OnRequestPrefab");
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
        msg.prefabIndex = playerPrefabIndex;
        client.Send(MsgTypes.PlayerPrefab, msg);
    }

    private void OnResponsePrefab(NetworkMessage netMsg)
    {
        Debug.Log("OnResponsePrefab");
        MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();
        playerPrefab = spawnPrefabs[msg.prefabIndex];
        base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
        Debug.Log(playerPrefab.name + " spawned!");

        if (fromServer)
        { 
            //GameObject.Find("PlayerPref(Clone)").name = DBManager.username;
            fromServer = false;
            Debug.Log("Remove auth from server");
            GameObject.Find("PresentationPlane(Clone)").GetComponent<NetworkIdentity>().RemoveClientAuthority(GameObject.Find(DBManager.username).GetComponent<NetworkIdentity>().connectionToClient);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("OnServerAddPlayer");
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = playerControllerId;
        NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);

        if (fromServer)
        { 
            SpawnObject(conn);
        }

        //GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //player.name = DBManager.username+"added";
        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    void SpawnObject(NetworkConnection conn)
    {
        playerPrefab = spawnPrefabs[4];
        var treeGo = Instantiate(playerPrefab);
        NetworkServer.SpawnWithClientAuthority(treeGo, conn);
    }

    // I have put a toggle UI on gameObjects called PC1 and PC2 to select two different character types.
    // on toggle, this function is called, which updates the playerPrefabIndex
    // The index will be the number from the registered spawnable prefabs that 
    // you want for your player
    public void UpdatePC()
    {
        Debug.Log("UpdatePC");
        if (GetComponent<NetworkManagerHUDCustom>().useMaleCharacter)
        {
            Debug.Log("UpdatePC" + 0);
            playerPrefabIndex = 0;
        }
        else //if (GameObject.Find("PC2").GetComponent<Toggle>().isOn)
        {
            Debug.Log("UpdatePC" + 1);
            playerPrefabIndex = 1;
        }

    }
        //public void OnClickHost()
        //{
        //    StartHost();
        //}

        //public void OnClickClient()
        //{
        // get the host IP from the input box
        //string hostIp = "localhost"; // TODO: for test

        // Set the networkAddress to the hostIp (this is inherited from NetworkManager)
        //networkAddress = hostIp;

        // Start the client
        //StartClient();
        //}
    }
