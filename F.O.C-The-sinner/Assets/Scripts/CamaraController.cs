using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraController : MonoBehaviour
{

    public Transform cameraRoot;

    public float mouseSensitivity = 2f;
    public float upLimit = -70f; 
    public float downLimit = 60f;

    public float smoothTime = 0.1f;

    private float xRotation = 0f;
    private float currentXRotation;
    private float xRotationVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mouseInput = Mouse.current.delta.ReadValue();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime * 0.1f;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime * 0.1f;
        Debug.Log("Input Y: " + mouseInput.y + " | Rotacion: " + xRotation);

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, upLimit, downLimit);

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, smoothTime);

        cameraRoot.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
    }

}
