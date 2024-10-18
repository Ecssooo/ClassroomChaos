using UnityEngine;
using System.Collections;

public class Teacher : MonoBehaviour
{
    [Header("Param�tres du Professeur")]
    [SerializeField] private float timeToShootTeacher = 10f;
    [SerializeField] private float valuePerHit = 5f;
    [SerializeField] private float accumulatedValue = 0f;
    [SerializeField] private AudioClip hitSound;

    private bool canBeShot = false;
    private bool isMoving = false;
    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.isKinematic = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        canBeShot = false;
    }

    public void ActivateTeacher()
    {
        // Appel� lorsque tous les �l�ves sont assomm�s
        canBeShot = true;
        Debug.Log("Vous pouvez maintenant tirer sur le professeur !");

        StartCoroutine(TeacherShootTimer());
    }

    private IEnumerator TeacherShootTimer()
    {
        float timer = timeToShootTeacher;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        ApplyAccumulatedForce();
    }

    private void ApplyAccumulatedForce()
    {
        if (!isMoving)
        {
            isMoving = true;

            rb.useGravity = true;
            rb.isKinematic = false;

            rb.velocity = new Vector3(0f, accumulatedValue / 2, accumulatedValue / 2);
            GameManager.Instance.GameState = GameStates.End;
            Debug.Log("Le professeur est propuls� avec la force accumul�e !");
        }
    }

    public void GetHit(GameObject projectile)
    {
        if (canBeShot)
        {
            accumulatedValue += valuePerHit;
            Debug.Log("Professeur touch� ! Valeur accumul�e : " + accumulatedValue);

            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            Destroy(projectile);
        }
        else
        {
            Debug.Log("Vous ne pouvez pas encore tirer sur le professeur !");
            Destroy(projectile);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GetHit(collision.gameObject);
        }
    }
}
