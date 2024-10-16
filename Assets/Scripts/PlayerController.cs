using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Sarbacane")]
    public GameObject sarbacanePrefab;
    public Transform sarbacaneSpawnPoint;
    private GameObject sarbacaneInstance;

    [Header("Boulette de Papier")]
    public GameObject boulettePrefab;
    public float bouletteSpeed = 50f;

    [Header("Camera")]
    public Camera playerCamera;
    public float rotationSpeed = 100f;

    [Header("UI")]
    public RectTransform viseurRectTransform; // Le RectTransform de l'Image du viseur

    private bool isAiming = false;

    void Start()
    {
        if (viseurRectTransform != null)
        {
            viseurRectTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleRotation();

        if (isAiming)
        {
            UpdateViseurPosition();
        }
    }

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (sarbacanePrefab != null && sarbacaneSpawnPoint != null)
            {
                isAiming = true;
                sarbacaneInstance = Instantiate(sarbacanePrefab, sarbacaneSpawnPoint.position, sarbacaneSpawnPoint.rotation, sarbacaneSpawnPoint);

                // Afficher le viseur
                if (viseurRectTransform != null)
                {
                    viseurRectTransform.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("il manque un prefab ABRUTI");
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            if (sarbacaneInstance != null)
            {
                Destroy(sarbacaneInstance);
            }

            // Masquer le viseur
            if (viseurRectTransform != null)
            {
                viseurRectTransform.gameObject.SetActive(false);
            }
        }
    }

    void HandleShooting()
    {
        if (isAiming && Input.GetMouseButtonDown(0))
        {
            // Tirer une boulette de papier
            if (boulettePrefab != null && sarbacaneSpawnPoint != null)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 targetPoint;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(1000); // vise loin direction souri
                }

                Vector3 direction = (targetPoint - sarbacaneSpawnPoint.position).normalized;

                GameObject boulette = Instantiate(boulettePrefab, sarbacaneSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = boulette.GetComponent<Rigidbody>();

                rb.velocity = direction * bouletteSpeed;
            }
            else
            {
                Debug.LogError("il manque un prefab IDIOT");
            }
        }
    }

    void HandleRotation()
    {
        if (!isAiming)
        {
            Vector3 mousePosition = Input.mousePosition;
            float screenWidth = Screen.width;

            if (mousePosition.x <= 0)
                mousePosition.x = 0;
            if (mousePosition.x >= screenWidth)
                mousePosition.x = screenWidth;

            float rotationDirection = 0f;

            if (mousePosition.x < screenWidth * 0.1f)
            {
                // Tourner à gauche
                rotationDirection = -1f;
            }
            else if (mousePosition.x > screenWidth * 0.9f)
            {
                // Tourner à droite
                rotationDirection = 1f;
            }

            transform.Rotate(0f, rotationDirection * rotationSpeed * Time.deltaTime, 0f);
        }
    }

    void UpdateViseurPosition()
    {
        if (viseurRectTransform != null)
        {
            // coordonée souri = coordonée canva ui (c'est chaud)
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viseurRectTransform.parent as RectTransform,
                Input.mousePosition,
                playerCamera,
                out mousePosition);

            // bouger le viseur
            viseurRectTransform.localPosition = mousePosition;
        }
    }
}
