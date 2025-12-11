using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraController : MonoBehaviour
{

    public Transform cameraRoot;

    [Header("Configuración")]
    public float mouseSensitivity = 200f;
    public float upLimit = -70f; // Límite para no desnucarse mirando arriba
    public float downLimit = 60f; // Límite para no mirar atravesando tus pies

    // Variables internas para guardar la rotación de la cámara
    private float xRotation = 0f; // Vertical 
    private float yRotation = 0f; // Horizontal 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Usamos la rotación del padre (el jugador) como base
        yRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        // 1. LEER EL RATÓN
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. ACUMULAR ROTACIÓN EN VARIABLES
        yRotation += mouseX;
        xRotation -= mouseY;

        // 3. LIMITAR LA VISTA VERTICAL
        xRotation = Mathf.Clamp(xRotation, upLimit, downLimit);

        // 4. APLICAR ROTACIÓN AL 'CAMERA ROOT'
        // Esto hace que la cámara gire alrededor del personaje,
        // PERO el cuerpo del personaje (transform) NO SE TOCA.
        // El cuerpo solo girará si usas el script de movimiento o si apuntas con el otro script.
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

}
