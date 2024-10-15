using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    
    [Header("Heart")]
    [SerializeField] private List<GameObject> _heartsList;

    private void Update()
    {
        switch (GameManager.Instance.GameState)
        {
            case(GameStates.StartScreen):
                break;
            case(GameStates.RoundInProgress):
                UpdateHeartsUI(GameManager.Instance.PlayerLife);
                break;
        }
    }
    
    public void UpdateHeartsUI(int life)
    {
        foreach (GameObject heart in _heartsList)
        {
            heart.SetActive(false);
            if (life >= 0)
            {
                heart.SetActive(true);
            }
            life--;
        }
    }
}
