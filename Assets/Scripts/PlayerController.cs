using System.Collections;
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
    public int bouletteNumber = 0;

    [Header("Camera")]
    public Camera playerCamera;
    public float rotationSpeed = 100f;

    [Header("UI")]
    public RectTransform viseurRectTransform; // Le RectTransform de l'Image du viseur

    [Header("Slider")]
    public Slider timerSlider;
    public float sliderTimer;
    public bool cantStop = false;
    public bool isReloading = false;

    private bool isAiming = false;

    void Start()
    {
        if (viseurRectTransform != null)
        {
            viseurRectTransform.gameObject.SetActive(false);
        }
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = 0;
        cantStop = false;
        StartTimer();
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleRotation();
        reload();

        if (isAiming)
        {
            UpdateViseurPosition();
        }
    }

    public void StartTimer()
    {
        StartCoroutine(StartTheTimerTicker());
    }

    IEnumerator StartTheTimerTicker()
    {
        while (timerSlider.value < sliderTimer)
        {
            if (cantStop == true)
            {
                timerSlider.value += Time.deltaTime;

                if (timerSlider.value >= sliderTimer - 0.1)
                {
                    bouletteNumber++;
                    timerSlider.value = 0;
                    cantStop = false;
                }
            }
            yield return null;
        }


    }

    void reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            cantStop = true;
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
            if (boulettePrefab != null && sarbacaneSpawnPoint != null && bouletteNumber > 0)
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
                bouletteNumber--;
            }
            else
            {
                Debug.Log("il te manque des boulette");
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
