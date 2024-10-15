using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [Header("ScreenUI")] 
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private GameObject _roundScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _endScreen;
        
        
    [Header("Heart")]
    [SerializeField] private List<GameObject> _heartsList;
    
    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    
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
            if (life > 0)
            {
                heart.SetActive(true);
            }
            life--;
        }
    }

    public void LoadUI(string uiType)
    {
        //Params : startscreen || roundscreen || losescreen || endscreen
        switch (uiType)
        {
            case("startscreen"):
                _startScreen.SetActive(true);
                break;
            case("roundscreen"):
                _roundScreen.SetActive(true);
                break;
            case("losescreen"):
                _loseScreen.SetActive(true);
                break;
            case("endscreen"):
                _endScreen.SetActive(true);
                break;
        }
    }
    
    public void UnLoadUI(string uiType)
    {
        //Params : startscreen || roundscreen || losescreen || endscreen
        switch (uiType)
        {
            case("startscreen"):
                _startScreen.SetActive(false);
                break;
            case("roundscreen"):
                _roundScreen.SetActive(false);
                break;
            case("losescreen"):
                _loseScreen.SetActive(false);
                break;
            case("endscreen"):
                _endScreen.SetActive(false);
                break;
        }
    }
}
