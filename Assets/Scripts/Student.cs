using UnityEngine;

public class Student : MonoBehaviour
{
    private bool isKnockedOut = false;

    public void GetHit()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            // Jouer une animation plus tard.

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boulette"))
        {
            GetHit();
            Destroy(collision.gameObject); // Détruire la boulette après impact
        }
    }
}
