using Enums;
using ObjectsPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class BallsController : PooledMonoBehaviour
    {
        public static event System.Action<int> OnBallsDestroy;

        [Header("Settings")]
        [SerializeField, Range(5f, 60f)] private float splitAngle = 25f;

        [Header("Dependencies")]
        [SerializeField] private LevelsManager levelsManager;
        [SerializeField] private PowerUpsManager powerUpsManager;

        public BallController FirstBallReadyToLaunch
        {
            get
            {
                foreach(Transform t in transform)
                {
                    if(t.TryGetComponent(out BallController ball))
                    {
                        if (t.gameObject.activeSelf && !ball.Launched)
                        {
                            return ball;
                        }
                    }
                }
                return null;
            }
        }

        private List<BallController> activeBalls = new();

        private void OnEnable()
        {
            BallController.OnBallDestroy += OnBallDestroy;
            PowerUpsManager.PowerUpActive += OnPowerUpActive;
            LifesManager.LifeLose += OnLifeLose;
        }
        private void OnDisable()
        {
            BallController.OnBallDestroy -= OnBallDestroy;
            PowerUpsManager.PowerUpActive -= OnPowerUpActive;
            LifesManager.LifeLose -= OnLifeLose;
        }

        protected override void OnGetPooledObject(PooledObject pooledObject)
        {
            BallController ball = (BallController)pooledObject;

            ball.Initialize(
                levelsManager.BaseBallSpeed,
                powerUpsManager
            );

            base.OnGetPooledObject(pooledObject);
        }

        private void OnLifeLose(int currentLifes)
        {
            if (currentLifes == 0)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                SpawnBall();
            }
        }

        private void OnPowerUpActive(EPowerUp powerUp)
        {
            if(powerUp == EPowerUp.Disruption)
            {
                SplitBalls();
            }
        }

        private void SplitBalls()
        {
            // Kopia pi³ek aktywnych przed podzia³em.
            var ballsToSplit = new List<BallController>(activeBalls);

            Transform paddleAnchor =
                GameManager.Instance.PaddleController.FireController.StartingPoint;

            foreach (BallController ball in ballsToSplit)
            {
                // Pomijamy pi³ki oczekuj¹ce na wystrzelenie.
                if (ball == null || !ball.Launched)
                    continue;

                Vector2 direction = ball.Direction;

                if (direction.sqrMagnitude < 0.0001f)
                    continue;

                Vector2 position = ball.transform.position;

                // Tworzymy dwie dodatkowe pi³ki.
                for (int i = -1; i <= 1; i += 2)
                {
                    float angle = splitAngle * i;

                    Vector2 newDirection = Quaternion.Euler(
                        0f, 0f, angle
                    ) * direction;

                    BallController newBall = Get(0) as BallController;

                    newBall.LaunchSplit(
                        paddleAnchor,
                        position,
                        newDirection
                    );
                    activeBalls.Add(newBall);
                }
            }
        }

        public void SpawnBall()
        {
            var ball = Get(0) as BallController;
            ball.OnSpawn(GameManager.Instance.PaddleController.FireController.StartingPoint);
            activeBalls.Add(ball);
        }

        private void OnBallDestroy(BallController ballController)
        {
            if (activeBalls.Contains(ballController))
                activeBalls.Remove(ballController);

            OnBallsDestroy?.Invoke(GetActiveCount());
        }

        internal void ClearBalls()
        {
            activeBalls.Clear();
            ReleaseAllActive();
        }
    }
}