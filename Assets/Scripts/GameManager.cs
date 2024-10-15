using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    [Header("GameStates")] 
    private GameStates _gameState;
    public GameStates GameState { get { return _gameState; } set { _gameState = value; } }

    private PlayerStates _playerState;
    public PlayerStates PlayerState { get { return _playerState; } set { _playerState = value; } }

    private TeacherStates _teacherState;
    public TeacherStates TeacherState { get { return _teacherState; } set { _teacherState = value; } }

    [Header("Timer")]
    [SerializeField] private float _maxTeacherTimer;
    [SerializeField] private float _minTeacherTimer;
    [SerializeField] private float _timerTeacherStayRegard;
    private float _currentTeacherTimer;
    
    
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

    public void Start()
    {
        _gameState = GameStates.StartScreen;
        _playerState = PlayerStates.WaitingScreen;
        _teacherState = TeacherStates.WaitingScreen;
    }

    public void Update()
    {
        GameLoop();
    }

    private void GameLoop()
    {
        switch (_gameState)
        {
            case(GameStates.StartScreen):
                
                Debug.Log("StartScreen");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _gameState = GameStates.RoundInProgress;
                }
                break;
            case(GameStates.RoundInProgress):
                Debug.Log("RoundInProgress");
                break;
            case(GameStates.LoseScreen):
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _gameState = GameStates.StartScreen;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
                Debug.Log("Lose");
                break;
            case(GameStates.End):
                Debug.Log("EndScreen");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _gameState = GameStates.StartScreen;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
                break;
        }
    }

    IEnumerator TeacherTimer(float timer)
    {
        //Params : Time teacher stay in Writing state
        yield return new WaitForSeconds(timer);
        _teacherState = TeacherStates.Regard;
        StartCoroutine(TeacherResetTimer(_timerTeacherStayRegard));
    }

    IEnumerator TeacherResetTimer(float timer)
    {
        //Params : Time teacher stay in Regard state
        yield return new WaitForSeconds(timer);
        _teacherState = TeacherStates.Writing;
        Random random = new Random();
        double rdm = random.NextDouble();
    }
}
