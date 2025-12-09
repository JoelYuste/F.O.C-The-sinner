using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraController : MonoBehaviour
{

    public Transform cameraRoot;

    [Header("Configuración")]
    public float mouseSensitivity = 200f;
    public float upLimit = -70f;
    public float downLimit = 60f;

    //  Cuanto más alto, más tarda el cuerpo en girar (más "peso")
    public float bodyLagTime = 0.15f;

    // Variables internas
    private float xRotation = 0f; // Mirar arriba/abajo
    private float yRotationTarget = 0f; // A donde QUEREMOS mirar (Ratón)
    private float currentYRotation = 0f; // Donde está el cuerpo REALMENTE
    private float rotateVelocity; // Auxiliar para el suavizado

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializamos las rotaciones para que no pegue un salto al empezar
        yRotationTarget = transform.eulerAngles.y;
        currentYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        // LEER EL RATÓN
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // CALCULAR OBJETIVOS 
        yRotationTarget += mouseX; // Sumamos el giro horizontal deseado

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, upLimit, downLimit);

        // MOVER EL CUERPO SUAVEMENTE
        currentYRotation = Mathf.SmoothDampAngle(currentYRotation, yRotationTarget, ref rotateVelocity, bodyLagTime);

        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // MOVER LA CÁMARA (Compensación)
        float cameraLocalY = yRotationTarget - currentYRotation;

        cameraRoot.localRotation = Quaternion.Euler(xRotation, cameraLocalY, 0f);
    }

}
