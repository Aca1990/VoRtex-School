using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class LogInManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var networkObjects = FindObjectsOfType<NetworkManagerCustom>();
        foreach (var obj in networkObjects)
        { 
            Destroy(obj.gameObject);
        }

        var networkObjectsForge = FindObjectsOfType<NetworkManager>();
        foreach (var obj in networkObjectsForge)
        {
            Destroy(obj.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
