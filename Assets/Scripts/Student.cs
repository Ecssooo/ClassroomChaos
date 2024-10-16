using UnityEngine;

public class Student : MonoBehaviour
{
    private bool isKnockedOut = false;

    public void GetHit()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            Debug.Log("C'est une dinguerie de faire ça !!!");
            // Jouer une animation plus tard.

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boulette"))
        {
            GetHit();
            Destroy(collision.gameObject); // la boulette disparaitra a l'aide d'un tour de magie
        }
    }
}
