using UnityEngine;

public class UserSetup : MonoBehaviour
{
    public string username;
    // Start is called before the first frame update
    void Start()
    {
        username = DBManager.username;
        PlayerPrefs.SetString("username", username);
        Debug.Log("user " + username + " loged in!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
