using System.Collections.Generic;
using UnityEngine;

public class ServerDataManager : MonoBehaviour
{
    private List<string> listOfActiveAchievements= new List<string> { "ProffesorVortexChat","MoveObject", "InteractWithUsers"};
    // Start is called before the first frame update
    void Start()
    {
        if (DBManager.LoggedIn)
        {
            Debug.Log("Welcome " + DBManager.username);
            Debug.Log("What have you achieve so far: " + DBManager.achievements);

            foreach (var a in DBManager.achievements)
            {
                if(listOfActiveAchievements.Contains(a))
                {
                    Debug.Log(a);
                }
            }
        }
    }
}
