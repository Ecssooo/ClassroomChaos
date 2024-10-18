using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Délai avant destruction")]
    [SerializeField]
    private float timeBeforeDestruction = 3f;

    private void Start()
    {
        Destroy(gameObject, timeBeforeDestruction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si le projectile touche le professeur alors que ce n'est pas autorisé, il doit être détruit sans effet
        if (collision.gameObject.CompareTag("Teacher"))
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null && playerController.CanShootTeacher)
            {
                // reaction du professeur
            }
            else
            {
                Debug.Log("Vous ne pouvez pas encore tirer sur le professeur !");
            }
            Destroy(gameObject);
        }
    }
}
