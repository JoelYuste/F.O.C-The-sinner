using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración")]
    public float speed = 5f;
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    public float turnSmoothTime = 0.1f; // Suavizado de giro

    [Header("Referencias")]
    public CharacterController controller;
    public CinemachineController cinemachineController;
    public Camera playerCamera; // Arrastra la Camera aquí

    // Variables privadas
    private Vector3 playerVelocity;
    private bool isGrounded;
    private Vector2 inputMovement; // Aquí guardamos lo que nos da OnMove
    private float turnSmoothVelocity;
    float targetAngle;
    float angle;

    private void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (playerCamera == null && Camera.main != null) playerCamera = Camera.main;
    }

    public void Update()
    {
        // 1. Gravedad
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Calculamos el ángulo de la cámara 
        targetAngle = playerCamera.transform.eulerAngles.y;

        // Giramos al personaje SIEMPRE hacia ese ángulo suavemente
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // 2.  (Viene de OnMove)
        Vector3 direction = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        // 3. Mover y Rotar con respecto a la cámara
        if (direction.magnitude >= 0.1f)
        {
            //// Ángulo hacia donde mira la cámara + tu input
            //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;

            //// Suavizar giro
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * direction;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 4. Aplicar Gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);



        RaycastHit disparo;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out disparo, 100f))
        {
            //Debug.Log("Has disparado a " + disparo.transform.name);
        }
        if (cinemachineController.aiming && Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out disparo, 100f);
            
            Debug.Log(disparo.point);
        }

    }


    // --- EVENTOS DEL INPUT SYSTEM ---

    public void OnMove(InputAction.CallbackContext context)
    {
        // Guardamos el valor (Vector2) en nuestra variable para usarla en Update
        inputMovement = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

    }
    


    private void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f);
        }
        
    }





}




