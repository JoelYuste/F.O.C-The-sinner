using UnityEngine;
using TMPro;

public class InteractCode : MonoBehaviour
{
    public GameObject message;
    public TextMeshPro textMessage;
    Vector2 screenCenterPoint;
    public float rangeRay;


    void Start()
    {
        screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, rangeRay))
        {
            Debug.DrawRay(ray.origin, ray.direction * rangeRay, Color.green);
            if (hit.collider.CompareTag("Interactable"))
            {
                message.SetActive(true);
                textMessage.text = hit.collider.GetComponent<GameObject>().tag;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.GetComponent<GameObject>();
                }
            }
            else
            {
                message.SetActive(false);
            }
        }
        else
        {
            message.SetActive(false);
        }

    }
}
