using System.Collections;
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
    
    [Header("Player Parameters")] 
    [SerializeField] private int _playerLife;
    public int PlayerLife { get { return _playerLife; } set { _playerLife = value; } }

    
    [Header("Teacher Parameters")]
    [SerializeField, Range(0,100)] private int _probabilityTeacherRegard;
    [SerializeField] private float _timerTeacherStayRegard;
    [SerializeField] private float teacherCooldown;
    public int ProbalitiyTeacherRegard { get { return _probabilityTeacherRegard; } set { _probabilityTeacherRegard = value; } }
    
    
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
                    //Init State
                    _gameState = GameStates.RoundInProgress;
                    _teacherState = TeacherStates.Writing;
                    _playerState = PlayerStates.Waiting;
                    
                    //Start Timer
                    StartCoroutine(TeacherTimer());
                    
                    //UIManager
                    UIManager.Instance.UnLoadUI("startscreen");
                    UIManager.Instance.LoadUI("roundscreen");
                }
                break;
            case(GameStates.RoundInProgress):
                Debug.Log("RoundInProgress");
                
                //End round Condition
                if (_playerLife <= 0)
                {
                    //Change State
                    _gameState = GameStates.LoseScreen;
                    _teacherState = TeacherStates.WaitingScreen;
                    _playerState = PlayerStates.WaitingScreen;
                    
                    //UIManager
                    UIManager.Instance.UnLoadUI("roundscreen");
                    UIManager.Instance.LoadUI("losescreen");
                }
                //Round Loop
                RoundLoop();
                break;
            case(GameStates.LoseScreen):
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Change State
                    _gameState = GameStates.StartScreen;
                    
                    //UIManager
                    UIManager.Instance.UnLoadUI("losescreen");
                    UIManager.Instance.LoadUI("startscreen");
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
                    //Change State
                    _gameState = GameStates.StartScreen;
                    
                    //UIManager
                    UIManager.Instance.UnLoadUI("endscreen");
                    UIManager.Instance.LoadUI("startscreen");
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
    
    
    IEnumerator TeacherTimer()
    {
        //
        yield return new WaitForSeconds(1);
        if (TeacherChangeState(_probabilityTeacherRegard) && _teacherState != TeacherStates.Cooldown)
        {
            _teacherState = TeacherStates.Regard;
            StartCoroutine(TeacherResetTimer());
        }
        else
        {
            StopCoroutine(TeacherTimer());
            StartCoroutine(TeacherTimer());
        }
    }

    IEnumerator TeacherResetTimer()
    {
        //Params : Time teacher stay in Regard state
        yield return new WaitForSeconds(_timerTeacherStayRegard);
        _teacherState = TeacherStates.Cooldown;
        StartCoroutine(TeacherOnCooldown());
    }

    IEnumerator TeacherOnCooldown()
    {
        yield return new WaitForSeconds(teacherCooldown);
        _teacherState = TeacherStates.Writing;
        StartCoroutine(TeacherTimer());
    }

    private bool TeacherChangeState(int probability)
    {
        Random random = new Random();
        int prob = random.Next(probability, 100);
        return prob == probability+1;
    }
}
