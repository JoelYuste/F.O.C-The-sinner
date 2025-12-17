using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CinemachineController : MonoBehaviour
{
    public enum CAMERA_MODE { BASE, AIM };

    #region Atributos
    [Header("Referencias")]
    public Transform chest; // Asigna el pecho o la cabeza del jugador aquí
    public bool aiming;

    private PlayerController pc;
    private Transform aimTarget;
    public float aimDistance = 10f; // Distancia del rayo de apuntado

    // Referencia a la cámara nueva
    private CinemachineCamera cCam;

    [Header("UI")]
    private Canvas canvas;
    private Image aimCursor;

    private float previousSpeed = -1;
    #endregion

    private void Awake()
    {
        
        cCam = FindAnyObjectByType<CinemachineCamera>();
        pc = FindAnyObjectByType<PlayerController>();

        var aimObj = GameObject.FindGameObjectWithTag("AimTarget");
        if (aimObj != null) aimTarget = aimObj.transform;

        canvas = FindAnyObjectByType<Canvas>();
        if (canvas != null)
        {
            var aim = canvas.transform.Find("AimCursor");
            if (aim != null) aimCursor = aim.GetComponent<Image>();
        }
    }

    private void Update() // Cambiado a Update para mejor respuesta de input
    {
        // Detectar si el jugador se mueve (para animaciones)
        bool walking = (pc != null && pc.GetComponent<CharacterController>().velocity.sqrMagnitude > 0.01f);

        // CLIC DERECHO para apuntar
        aiming = Input.GetMouseButton(1);

        // Posicionar el objetivo de apuntado en el centro de la pantalla
        if (aimTarget != null && Camera.main != null)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, aimDistance);
            aimTarget.position = Camera.main.ScreenToWorldPoint(screenCenter);
        }

        if (aiming)
        {
            // MODO APUNTADO
            SetCameraMode(CAMERA_MODE.AIM);

            // Detener al personaje al apuntar (opcional)
            if (pc != null)
            {
                if (previousSpeed < 0) previousSpeed = pc.speed;
                pc.speed = 0; // O pon una velocidad muy lenta (ej: 1f)
            }

            // Rotar el personaje hacia donde mira la cámara
            if (aimTarget != null && chest != null)
            {
                // Calculamos la dirección ignorando la altura para que no se incline raro
                Vector3 aimVector = aimTarget.position - pc.transform.position;
                aimVector.y = 0;

                if (aimVector != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(aimVector);
                    pc.transform.rotation = Quaternion.Slerp(pc.transform.rotation, rotation, Time.deltaTime * 10f);
                }
            }
        }
        else
        {
            // MODO NORMAL
            SetCameraMode(CAMERA_MODE.BASE);

            // Restaurar velocidad
            if (pc != null && previousSpeed >= 0)
            {
                pc.speed = previousSpeed;
                previousSpeed = -1;
            }
        }

        // Animaciones
        if (pc != null)
        {
            var anim = pc.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("IsWalking", walking);
                anim.SetBool("IsAiming", aiming);
            }
        }

        // Mostrar/Ocultar mira
        if (aimCursor != null)
            aimCursor.gameObject.SetActive(aiming);
    }

    // AQUI ESTA LA CORRECCION CLAVE PARA UNITY 6
    private void SetCameraMode(CAMERA_MODE mode)
    {
        if (cCam == null) return;

        // Buscamos el componente ThirdPersonFollow (que controla la distancia y el hombro)
        var thirdPerson = cCam.GetComponent<CinemachineThirdPersonFollow>();

        if (thirdPerson == null)
        {
            Debug.LogWarning("¡Tu cámara no tiene el componente CinemachineThirdPersonFollow!");
            return;
        }

        switch (mode)
        {
            case CAMERA_MODE.BASE:
                // Valores normales
                thirdPerson.CameraDistance = 2f;           // Lejos
                thirdPerson.ShoulderOffset = new Vector3(0.28f, -0.31f, 0.46f); // Un poco a la derecha
                thirdPerson.VerticalArmLength =0.4f;        // Altura normal
                break;

            case CAMERA_MODE.AIM:
                // Valores de apuntado (Zoom)
                thirdPerson.CameraDistance = 1.5f;           // Cerca
                thirdPerson.ShoulderOffset = new Vector3(1.2f, 0.2f, 0f); // Muy a la derecha (sobre el hombro)
                thirdPerson.VerticalArmLength = 0.8f;        // Un poco más bajo o ajustado
                break;
        }
    }
}
