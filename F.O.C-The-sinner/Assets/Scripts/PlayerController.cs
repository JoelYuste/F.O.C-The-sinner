using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;
    public CharacterController controller;
    public float jump = 8f;
    private Vector3 velocidadVertical;
    private Vector3 moveDirection;
    public bool isGrounded;
    public float gravityValue = -9.81f;
    public Camera playerCamera;



    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = 0f;
        }

        Vector3 movimiento = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        controller.Move(movimiento * speed * Time.deltaTime);

        velocidadVertical.y += gravityValue * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocidadVertical.y = Mathf.Sqrt(jump * -2f * gravityValue);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    

    










}
