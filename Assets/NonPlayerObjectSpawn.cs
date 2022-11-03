using UnityEngine;
using UnityEngine.Networking;

public class NonPlayerObjectSpawn : MonoBehaviour
{
    public GameObject treePrefab;

    void SpawnObject(NetworkConnection conn)
    {
        var treeGo = Instantiate(treePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(treeGo, conn);
    }
}
