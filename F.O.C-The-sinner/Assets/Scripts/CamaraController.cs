using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraController : MonoBehaviour
{

    public Transform cameraRoot;

    [Header("Configuración")]
    public float mouseSensitivity = 200f;


    // Variables internas
    private float xRotation = 0f; // Mirar arriba/abajo
    private float yRotationTarget = 0f; // A donde QUEREMOS mirar (Ratón)
    private float currentYRotation = 0f; // Donde está el cuerpo REALMENTE

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

       
       //yRotationTarget = transform.eulerAngles.y;
       // currentYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        // LEER EL RATÓN
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // CALCULAR OBJETIVOS 
        yRotationTarget += mouseX; // Sumamos el giro horizontal deseado
        xRotation -= mouseY;


        //xRotation = Mathf.Clamp(xRotation, upLimit, downLimit);

        transform.Rotate(Vector3.up * mouseX);

        //transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // MOVER LA CÁMARA (Compensación)
        float cameraLocalY = yRotationTarget - currentYRotation;

        cameraRoot.localRotation = Quaternion.Euler(xRotation, cameraLocalY, 0f);
    }

}
