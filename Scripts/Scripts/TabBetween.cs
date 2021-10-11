using UnityEngine;
using UnityEngine.UI;

public class TabBetween : MonoBehaviour
{
    public InputField nextField;
    InputField myField;

    // Start is called before the first frame update
    void Start()
    {
        if(nextField == null)
        {
            Destroy(this);
            return;
        }
        myField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myField.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            nextField.ActivateInputField();
        }
    }
}
