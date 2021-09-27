using UnityEngine;
using UnityEngine.UI;

public class ChatInputFieldManager : MonoBehaviour
{
    public ChatterManager chatterManager;
    private InputField inputField;

    private void Start()
    {
        inputField = GetComponent<InputField>();
    }

    public void ValueChanged()
    {
        if(inputField.text.Contains("\n"))
        {
            chatterManager.WriteMessage(inputField);
        }
    }
}
