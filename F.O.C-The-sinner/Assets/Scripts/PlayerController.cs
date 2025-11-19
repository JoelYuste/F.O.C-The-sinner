using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public CharacterController controller;
    public float Jump = 8f;
    public Vector3 moveDirection;
    public bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection.y = 0f; // Reset vertical velocity when grounded
        }
        moveDirection.y += Physics.gravity.y * Time.deltaTime; // Apply gravity
        controller.Move(moveDirection * Time.deltaTime); // Move the character

        moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            moveDirection.y = Jump;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
    }

    








}
