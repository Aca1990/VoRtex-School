using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
    public float speed = 10.0F;
    float rotationSpeed = 50.0F;
    public static bool EnableController;
    Animator animator;

    void Start(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        EnableController = true;
        animator.SetBool("Idling", true);
    }
	
    // Update is called once per frame
	void FixedUpdate ()
    {
        if (EnableController)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;
            Quaternion turn = Quaternion.Euler(0f,rotation,0f);
            rb.MovePosition(rb.position + this.transform.forward * translation);
            rb.MoveRotation(rb.rotation * turn);

            if(translation != 0) 
            {
                animator.SetBool("Idling", false);
                GetComponent<SetupLocalPlayer>().CmdChangeAnimState("run");
            }
            else
            {
                animator.SetBool("Idling", true);
                GetComponent<SetupLocalPlayer>().CmdChangeAnimState("idle");
            }
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name == "InputField") // TODO: check other gui objects
            {
                EnableController = false;
                ChangeCamera.CameraActive = false;
            }
            else
            {
                EnableController =true;
                ChangeCamera.CameraActive = true;
            }
        }

    }
}
