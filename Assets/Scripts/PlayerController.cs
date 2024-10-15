using UnityEngine;

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

    private bool isAiming = false;

    void Update()
    {
        HandleAiming();
        HandleShooting();
        HandleRotation();
    }

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Activer la sarbacane et le viseur
            isAiming = true;
            GameManager.Instance.PlayerState = PlayerStates.Shooting;
            sarbacaneInstance = Instantiate(sarbacanePrefab, sarbacaneSpawnPoint.position, sarbacaneSpawnPoint.rotation, sarbacaneSpawnPoint);
            // Afficher le viseur (ui, comme ça il reste au centre de l'écran)
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Désactiver la sarbacane et le viseur
            isAiming = false;
            Destroy(sarbacaneInstance);
            GameManager.Instance.PlayerState = PlayerStates.Waiting;
            // Masquer le viseur
        }
    }

    void HandleShooting()
    {
        if (isAiming && Input.GetMouseButtonDown(0))
        {
            // Tirer une boulette de papier
            GameObject boulette = Instantiate(boulettePrefab, sarbacaneSpawnPoint.position, sarbacaneSpawnPoint.rotation);
            Rigidbody rb = boulette.GetComponent<Rigidbody>();
            rb.velocity = playerCamera.transform.forward * bouletteSpeed;
        }
    }

    void HandleRotation()
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
