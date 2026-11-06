using Ball;
using PowerUps;
using Saving;
using StateMachine.Game;
using UI;
using UnityEngine;
using Enums;
using Paddle;
using Enemies;
using static UnityEngine.Rendering.STP;
using Enemies.Boss;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int CurrentLevelID { get; private set; } = 0;
    public bool IsWin { get; private set; } = false;

    public BallsController BallsController => ballsController;
    public PaddleController PaddleController => paddleController;
    public ScoreManager ScoreManager => scoreManager;
    public GameStateMachine StateMachine => stateMachine;
    public UIManager UIManager => uiManager;
    public LevelsManager LevelsManager => levelsManager;
    public LifesManager LifesManager => lifesManager;
    public PowerUpsManager PowerUpsManager => powerUpsManager;
    public EnemiesManager EnemiesManager => enemiesManager;
    public BossManager BossManager => bossManager;
    public AudioController AudioController => audioController;

    [SerializeField] private PaddleController paddleController;
    [SerializeField] private BallsController ballsController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameStateMachine stateMachine;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelsManager levelsManager;
    [SerializeField] private LifesManager lifesManager;
    [SerializeField] private PowerUpsManager powerUpsManager;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private BossManager bossManager;
    [SerializeField] private AudioController audioController;

    [SerializeField] private int startLevel = 0;

    private void Start()
    {
        SaveSystem.Load();
        StateMachine.RegisterStates();
        uiManager.Setup();
    }

    public void BackToMainMenu()
    {
        StateMachine.ChangeState(EGameState.MainMenu);
    }

    public void StartNewGame()
    {
        CurrentLevelID = startLevel;

        LifesManager.RestoreLifes();
        StateMachine.ChangeState(EGameState.Prepare);
        ScoreManager.RestartScore();
    }
    public void LoadGame()
    {
        CurrentLevelID = SaveSystem.Data.HighestUnlockedLevel;
        LifesManager.RestoreLifes();
        StateMachine.ChangeState(EGameState.Prepare);
    }

    internal void GameOver()
    {
        IsWin = false;

        if (CurrentLevelID > SaveSystem.Data.HighestUnlockedLevel)
            SaveSystem.Data.HighestUnlockedLevel = CurrentLevelID;

        SaveSystem.Save();
        StateMachine.ChangeState(EGameState.Summary);
    }

    internal void EnableNextLevel()
    {
        if (CurrentLevelID == LevelsManager.GetMaxLevel())
        {
            WinEndGame();
            return;
        }

        CurrentLevelID++;
        SaveSystem.Data.HighestUnlockedLevel = CurrentLevelID;
        SaveSystem.Save();
        StateMachine.ChangeState(EGameState.Prepare);
    }

    internal void WinEndGame()
    {
        IsWin = true;
        SaveSystem.Data.HighestUnlockedLevel = CurrentLevelID;

        SaveSystem.Save();
        StateMachine.ChangeState(EGameState.Summary);
    }

    //public void StartNewGame()
    //{
    //    BallsController.SpawnBall();
    //}
}