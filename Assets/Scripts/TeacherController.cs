using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public class TeacherController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GameManager.Instance.TeacherRigidbody;
    }

    public IEnumerator TeacherTimer()
    {
        //
        yield return new WaitForSeconds(1);
        if (TeacherChangeState(GameManager.Instance.CurrentProbaTeacherRegard) && GameManager.Instance.TeacherState != TeacherStates.Cooldown)
        {
            StartCoroutine(RotateTeacherInRegard());
            StartCoroutine(TeacherResetTimer());
        }
        else
        {
            StopCoroutine(TeacherTimer());
            StartCoroutine(TeacherTimer());
        }
    }
    
    public IEnumerator TeacherResetTimer()
    {
        yield return new WaitForSeconds(GameManager.Instance.TimerTeacherStayRegard);
        GameManager.Instance.TeacherState = TeacherStates.Cooldown;
        StartCoroutine(TeacherOnCooldown());
    }

    public IEnumerator TeacherOnCooldown()
    {
        StartCoroutine(RotateTeacherInCooldown());
        yield return new WaitForSeconds(GameManager.Instance.TeacherCooldown);
        GameManager.Instance.TeacherState = TeacherStates.Writing;
        StartCoroutine(TeacherTimer());
    }

    public void ModifyTeacherProbaByNoise()
    {
        if (GameManager.Instance.NoiseLevel >= GameManager.Instance.ThirdLevelNoise)
        {
            GameManager.Instance.CurrentProbaTeacherRegard = GameManager.Instance.ThirdLevelProba;
        }
        else if (GameManager.Instance.NoiseLevel <= GameManager.Instance.FirstLevelNoise 
                  && GameManager.Instance.NoiseLevel >= GameManager.Instance.SecondeLevelNoise)
        {
            GameManager.Instance.CurrentProbaTeacherRegard = GameManager.Instance.SecodeLevelProba;
        }
        else if (GameManager.Instance.NoiseLevel < GameManager.Instance.SecondeLevelNoise)
        {
            GameManager.Instance.CurrentProbaTeacherRegard = GameManager.Instance.FirstLevelNoise;
        }
    }
    
    public bool TeacherChangeState(int probability)
    {
        //Calcul teacher rotate probability
        Random random = new Random();
        int prob = random.Next(probability, 100);
        return prob == probability+1;
    }

    public IEnumerator RotateTeacherInRegard()
    {
        //Rotate Teacher in front of the classroom
        rb.DORotate(new Vector3(0, 180, 0), 1f);
        yield return new WaitForSeconds(1);
        GameManager.Instance.TeacherState = TeacherStates.Regard;
    }

    public IEnumerator RotateTeacherInCooldown()
    {
        //Rotate Teacher in front of the board
        rb.DORotate(new Vector3(0, 0, 0), 1f);
        yield return new WaitForSeconds(1);
        GameManager.Instance.TeacherState = TeacherStates.Cooldown;
    }
}
