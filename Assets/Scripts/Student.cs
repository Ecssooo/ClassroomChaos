using System;
using UnityEngine;

public class Student : MonoBehaviour
{
    public bool IsKnockedOut { get; private set; } = false;
    public Animator anim;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStates.StartScreen)
            IsKnockedOut = false;
    }

    public void GetHit(GameObject projectile)
    {
        if (!IsKnockedOut)
        {
            IsKnockedOut = true;
            Debug.Log("L'élève est assommé !");

            anim.SetTrigger("isDead");

            GameManager.Instance.NoiseController.PlayRandomHitSound();
            GameManager.Instance.NoiseController.IncreaseNoiseLevel(GameManager.Instance.HitNoise);

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
