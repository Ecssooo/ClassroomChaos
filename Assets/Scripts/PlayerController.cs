using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShootableObject
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float probability;
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Objets � lancer")]
    [SerializeField]
    private List<ShootableObject> shootableObjects;

    [Header("Param�tres de tir")]
    public float objectSpeed = 50f;
    public int ammoCount = 0;

    [Header("Camera")]
    public Camera playerCamera;
    public float rotationSpeed = 100f;

    [Header("Limites de rotation")]
    public float minYRotation = -60f;
    public float maxYRotation = 60f;
    private float currentYRotation = 0f;

    [Header("UI")]
    public RectTransform viseurRectTransform;
    public TextMeshProUGUI ammoText;

    [Header("Slider")]
    public Slider timerSlider;
    public float sliderTimer;

    [Header("Sons")]
    public AudioClip reloadSound;
    public AudioSource audioSource;

    private bool isReloading = false;
    private bool isAiming = false;

    private bool canShootTeacher = false;

    public bool CanShootTeacher
    {
        get { return canShootTeacher; }
    }

    private Teacher teacher;

    #endregion

    #region M�thodes Unity

    void Start()
    {
        if (viseurRectTransform != null)
        {
            viseurRectTransform.gameObject.SetActive(false);
        }

        if (timerSlider != null)
        {
            timerSlider.maxValue = sliderTimer;
            timerSlider.value = 0;
            timerSlider.gameObject.SetActive(false);
        }

        teacher = FindObjectOfType<Teacher>();

        GameManager.Instance.PlayerState = PlayerStates.Waiting;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        UpdateAmmoText();
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameStates.StartScreen)
            ammoCount = 3;
        
        HandleAiming();
        HandleShooting();
        HandleRotation();
        HandleReload();
        CheckStudentsStatus();

        if (isAiming)
        {
            UpdateViseurPosition();
        }

        if (!isAiming && !isReloading && GameManager.Instance.PlayerState != PlayerStates.Waiting)
        {
            GameManager.Instance.PlayerState = PlayerStates.Waiting;
        }
    }

    #endregion

    #region Gestion du Rechargement

    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isAiming)
        {
            isReloading = true;
            GameManager.Instance.PlayerState = PlayerStates.Reloading;

            if (reloadSound != null)
            {
               
                audioSource.PlayOneShot(reloadSound);
            }

            if (timerSlider != null)
            {
                timerSlider.gameObject.SetActive(true);
                timerSlider.value = 0;
            }
            StartCoroutine(StartReloadingTimer());
        }
    }

    IEnumerator StartReloadingTimer()
    {
        while (isReloading)
        {
            if (timerSlider != null)
            {
                timerSlider.value += Time.deltaTime;
            }

            if (timerSlider != null && timerSlider.value >= sliderTimer)
            {
                FinishReloading();
                yield break;
            }

            yield return null;
        }
    }

    void FinishReloading()
    {
        isReloading = false;
        ammoCount++;
        UpdateAmmoText();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        if (timerSlider != null)
        {
            timerSlider.value = 0;
            timerSlider.gameObject.SetActive(false);
        }
        GameManager.Instance.PlayerState = PlayerStates.Waiting;
    }

    #endregion

    #region Gestion de la Vis�e

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1) && !isReloading)
        {
            isAiming = true;

            if (viseurRectTransform != null)
            {
                viseurRectTransform.gameObject.SetActive(true);
            }

            GameManager.Instance.PlayerState = PlayerStates.Shooting;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;

            if (viseurRectTransform != null)
            {
                viseurRectTransform.gameObject.SetActive(false);
            }

            if (GameManager.Instance.PlayerState == PlayerStates.Shooting)
            {
                GameManager.Instance.PlayerState = PlayerStates.Waiting;
            }
        }
    }

    void UpdateViseurPosition()
    {
        if (viseurRectTransform != null)
        {
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viseurRectTransform.parent as RectTransform,
                Input.mousePosition,
                playerCamera,
                out mousePosition);

            viseurRectTransform.localPosition = mousePosition;
        }
    }

    #endregion

    #region Gestion du Tir

    void HandleShooting()
    {
        if (isAiming && Input.GetMouseButtonDown(0))
        {
            if (shootableObjects != null && shootableObjects.Count > 0)
            {
                if (!canShootTeacher && ammoCount <= 0)
                {
                    Debug.Log("Vous n'avez pas de munitions ! Rechargez pour en obtenir.");
                    return;
                }

                GameObject selectedPrefab = SelectRandomObject();

                if (selectedPrefab != null)
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
                        targetPoint = ray.GetPoint(1000);
                    }

                    Vector3 direction = (targetPoint - playerCamera.transform.position).normalized;

                    GameObject projectile = Instantiate(selectedPrefab, playerCamera.transform.position, Quaternion.identity);
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();

                    rb.velocity = direction * objectSpeed;

                    if (!canShootTeacher)
                    {
                        ammoCount--;
                        UpdateAmmoText();
                    }

                    Debug.Log("Tir effectu� !");
                }
                else
                {
                    Debug.LogError("Aucun objet s�lectionn� pour le tir.");
                }
            }
            else
            {
                Debug.Log("Aucun objet � lancer.");
            }
        }
    }

    GameObject SelectRandomObject()
    {
        float totalProbability = 0f;
        foreach (var obj in shootableObjects)
        {
            totalProbability += obj.probability;
        }

        float randomPoint = Random.value * totalProbability;

        foreach (var obj in shootableObjects)
        {
            if (randomPoint < obj.probability)
            {
                return obj.prefab;
            }
            else
            {
                randomPoint -= obj.probability;
            }
        }
        return null;
    }

    #endregion

    #region Gestion de la Rotation

    void HandleRotation()
    {
        if (!isAiming)
        {
            Vector3 mousePosition = Input.mousePosition;
            float screenWidth = Screen.width;

            float rotationDirection = 0f;

            if (mousePosition.x < screenWidth * 0.1f)
            {
                rotationDirection = -1f;
            }
            else if (mousePosition.x > screenWidth * 0.9f)
            {
                rotationDirection = 1f;
            }

            float rotationAmount = rotationDirection * rotationSpeed * Time.deltaTime;
            currentYRotation += rotationAmount;

            currentYRotation = Mathf.Clamp(currentYRotation, minYRotation, maxYRotation);

            transform.localEulerAngles = new Vector3(0f, currentYRotation, 0f);
        }
    }

    #endregion

    #region V�rification des �l�ves

    void CheckStudentsStatus()
    {
        if (!canShootTeacher)
        {
            Student[] students = FindObjectsOfType<Student>();
            bool allKnockedOut = true;

            foreach (Student student in students)
            {
                if (!student.IsKnockedOut)
                {
                    allKnockedOut = false;
                    break;
                }
            }

            if (allKnockedOut)
            {
                canShootTeacher = true;
                UpdateAmmoText();
                Debug.Log("ATOMISE LA PROF");
                if (teacher != null)
                {
                    teacher.ActivateTeacher();
                }
            }
        }
    }

    #endregion

    #region Mise � jour de l'UI

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            if (canShootTeacher)
            {
                ammoText.text = "Munitions : Illimit�";
            }
            else
            {
                ammoText.text = ammoCount.ToString();
            }
        }
    }

    #endregion
}
