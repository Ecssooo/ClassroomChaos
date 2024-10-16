using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TeacherController : MonoBehaviour
{
    public IEnumerator TeacherTimer()
    {
        //
        yield return new WaitForSeconds(1);
        if (TeacherChangeState(GameManager.Instance.CurrentProbaTeacherRegard) && GameManager.Instance.TeacherState != TeacherStates.Cooldown)
        {
            GameManager.Instance.TeacherState = TeacherStates.Regard;
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
        //Params : Time teacher stay in Regard state
        yield return new WaitForSeconds(GameManager.Instance.TimerTeacherStayRegard);
        GameManager.Instance.TeacherState = TeacherStates.Cooldown;
        StartCoroutine(TeacherOnCooldown());
    }

    public IEnumerator TeacherOnCooldown()
    {
        
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
        Random random = new Random();
        int prob = random.Next(probability, 100);
        return prob == probability+1;
    }
}
