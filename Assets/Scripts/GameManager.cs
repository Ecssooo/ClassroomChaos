using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    #region Teacher parameters
    [Header("Teacher Parameters")] 
    [SerializeField] private Rigidbody _teacherRigidbody;
    [SerializeField] private TeacherController _teacherController;
    [SerializeField, Range(0,100)] private int _probabilityTeacherRegard;
    [SerializeField] private float _timerTeacherStayRegard;
    [SerializeField] private float teacherCooldown;
    private bool _alreadyLose;
    
    #region Accesseurs    
    private int _currentProbaTeacherRegard;
    public int CurrentProbaTeacherRegard { get { return _currentProbaTeacherRegard; } set { _currentProbaTeacherRegard = value; } }
    public int ProbabilityTeacherRegard { get { return _probabilityTeacherRegard; } }
    public float TimerTeacherStayRegard { get { return _timerTeacherStayRegard; } }
    public float TeacherCooldown { get { return teacherCooldown; } }
    public Rigidbody TeacherRigidbody { get { return _teacherRigidbody; } }
    #endregion
    #endregion

    #region Noise Level Probability
    [Header("Probability by Noise")]
    [SerializeField] private int _firstLevelNoise;
    [SerializeField] private int _secondeLevelNoise;
    [SerializeField] private int _thirdLevelNoise;
    [SerializeField] private int _firstLevelProba;
    [SerializeField] private int _secondeLevelProba;
    [SerializeField] private int _thirdLevelProba;
    
    #region Accesseurs
    
    public int FirstLevelNoise { get { return _firstLevelNoise; } }
    public int SecondeLevelNoise { get { return _secondeLevelNoise; } }
    public int ThirdLevelNoise { get { return _thirdLevelNoise; } }
    public int FirstLevelProba { get { return _firstLevelProba; } }
    public int SecodeLevelProba { get { return _secondeLevelProba; } }
    public int ThirdLevelProba { get { return _thirdLevelProba; } }

    #endregion
    #endregion
    
    #region NoiseController
    [Header("Noise Controller")]
    [SerializeField] private NoiseController _noiseController;
    private float _noiseLevel;
    [SerializeField] private float _decreaseNoiseLevel;
    [SerializeField] private float _craftNoise;
    [SerializeField] private float _shootNoise;
    [SerializeField] private float _hitNoise;
    [SerializeField] private float _missNoise;
    
    #region Accesseurs
    public NoiseController NoiseController { get { return _noiseController; } }
    public float NoiseLevel { get { return _noiseLevel; } set { _noiseLevel = value; } }
    public float CraftNoise { get { return _craftNoise; } }
    public float ShootNoise { get { return _shootNoise; } }
    public float HitNoise { get { return _hitNoise; } }
    public float MissNoise { get { return _missNoise; } }
    
    #endregion
    #endregion
    
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
        //Init States
        _gameState = GameStates.StartScreen;
        _playerState = PlayerStates.WaitingScreen;
        _teacherState = TeacherStates.WaitingScreen;
        
        //Init Variables
        _currentProbaTeacherRegard = _probabilityTeacherRegard;
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
                    StartCoroutine(_teacherController.TeacherTimer());
                    
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
                _alreadyLose = false;
                break;
            case(TeacherStates.Regard):
                Debug.Log("Teacher Regard");
                if ((_playerState == PlayerStates.Shooting ||
                    _playerState == PlayerStates.Reloading ||
                    _playerState == PlayerStates.Crafting) && !_alreadyLose)
                {
                    _playerLife--;
                    _alreadyLose = true;
                }
                break;
        }
        _noiseController.DecreaseNoiseLevel(_decreaseNoiseLevel/100);
        _teacherController.ModifyTeacherProbaByNoise();
    }

    public void DestroyGameManager()
    {
        //Use this function before reload / change scene;
        Destroy(gameObject);
    }
}
