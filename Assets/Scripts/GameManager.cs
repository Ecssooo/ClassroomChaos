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
    [SerializeField] private int _maxTeacherTimer;
    [SerializeField] private int _minTeacherTimer;
    [SerializeField] private float _timerTeacherStayRegard;
    private float _currentTeacherTimer;

    [Header("Player")] 
    [SerializeField] private int _playerLife;
    public int PlayerLife { get { return _playerLife; } set { _playerLife = value; } }
    
    
    
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
                    _teacherState = TeacherStates.Writing;
                    _playerState = PlayerStates.Waiting;
                    StartCoroutine(TeacherTimer(GenerateFloat(_minTeacherTimer, _maxTeacherTimer)));
                    
                }
                break;
            case(GameStates.RoundInProgress):
                Debug.Log("RoundInProgress");
                if (_playerLife <= 0)
                    _gameState = GameStates.LoseScreen;
                RoundLoop();
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

    private void RoundLoop()
    {
        switch (_teacherState)
        {
            case(TeacherStates.Writing):
                Debug.Log("Teacher Writing");
                break;
            case(TeacherStates.Regard):
                Debug.Log("Teacher Regard");
                if (_playerState == PlayerStates.Shooting ||
                    _playerState == PlayerStates.Reloading ||
                    _playerState == PlayerStates.Crafting)
                {
                    _playerLife--;
                }
                break;
        }
    }

    public void DestroyGameManager()
    {
        //Use this function before reload / change scene;
        Destroy(gameObject);
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
        StartCoroutine(TeacherTimer(GenerateFloat(_minTeacherTimer, _maxTeacherTimer)));
    }

    private float GenerateFloat(int min,int max)
    {
        Random random = new Random();
        int unit = random.Next(min, max);
        float dec = random.Next(0, 99) * 0.01f;
        return unit + dec;
    }
}
