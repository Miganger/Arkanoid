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

    public BallsController BallsController => ballsController;
    public PaddleController PaddleController => paddleController;
    public ScoreManager ScoreManager => scoreManager;
    public GameStateMachine StateMachine => stateMachine;
    public UIManager UIManager => uiManager;
    public LevelsManager LevelsManager => levelsManager;
    public LifesManager LifesManager => lifesManager;
    public PowerUpsManager PowerUpsManager => powerUpsManager;
    public EnemiesManager EnemiesManager => enemiesManager;

    [SerializeField] private PaddleController paddleController;
    [SerializeField] private BallsController ballsController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameStateMachine stateMachine;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelsManager levelsManager;
    [SerializeField] private LifesManager lifesManager;
    [SerializeField] private PowerUpsManager powerUpsManager;
    [SerializeField] private EnemiesManager enemiesManager;

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

        SaveSystem.Data.HighestUnlockedLevel = CurrentLevelID;
        SaveSystem.Save();

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
        SaveSystem.Data.HighestUnlockedLevel = 0;
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
        StateMachine.ChangeState(EGameState.MainMenu);
    }

    //public void StartNewGame()
    //{
    //    BallsController.SpawnBall();
    //}
}