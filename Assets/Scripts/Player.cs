using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 look;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float movement = 10f;
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float mass = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float acceleration = 20f;
    CharacterController characterController;
    Vector3 velocity;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];
        jumpAction = playerInput.actions["jump"];

    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        UpdateLook();



    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;

        velocity.y = characterController.isGrounded ? -1f : velocity.y + gravity.y;
    }

    Vector3 GetMovementInput()
    {
        var moveInput = moveAction.ReadValue<Vector2>();
        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= movement;
        return input;

    }

    void UpdateMovement()
    {
        //var x = Input.GetAxis("Horizontal");
        //var y = Input.GetAxis("Vertical");

        var input = GetMovementInput();

        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        var jumpInput = jumpAction.ReadValue<float>();

        if (jumpInput > 0 && characterController.isGrounded)
        {
            velocity.y += jumpSpeed;
        }
        //transform.Translate(input * movement * Time.deltaTime, Space.World);
        characterController.Move(velocity * Time.deltaTime);
    }

    void UpdateLook()
    {
        var lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensitivity;
        look.y += lookInput.y * mouseSensitivity;
        look.y = Mathf.Clamp(look.y, -89f, 89f);
        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);

    }
}
