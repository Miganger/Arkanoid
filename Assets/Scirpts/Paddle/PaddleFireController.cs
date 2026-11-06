using ObjectsPool;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paddle
{
    public class PaddleFireController : PooledMonoBehaviour
    {
        public static event System.Action LunchBall;
        public static event System.Action LaserFire;

        public Transform StartingPoint => startingPoint;

        [Header("Settings")]
        [SerializeField] private float cooldown = 1f;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference powerAction;

        [Header("References")]
        [SerializeField] private Transform startingPoint;
        [SerializeField] private Transform leftLaserPos;
        [SerializeField] private Transform rightLaserPos;
 
        private float timer = 0;

        public void EnableActions()
        {
            powerAction.action.Enable();
        }
        public void DisableActions()
        {
            powerAction.action.Disable();
        }

        public void ClearMissles()
        {
            ReleaseAllActive();
        }

        private void Fire()
        {
            var leftMissle = Get() as Missle;
            leftMissle.transform.position = leftLaserPos.position;

            var rightMissle = Get() as Missle;
            rightMissle.transform.position = rightLaserPos.position;

            leftMissle.Fire();
            rightMissle.Fire();

            LaserFire?.Invoke();

            timer = cooldown;
        }

        private void Update()
        {
            if (powerAction.action.triggered)
            {
                var unLunchedBall = GameManager.Instance.BallsController.FirstBallReadyToLaunch;

                if (unLunchedBall)
                {
                    LunchBall?.Invoke();
                    unLunchedBall.Launch();
                }
                else
                {
                    if (timer > 0)
                        return;

                    if (GameManager.Instance.PowerUpsManager.CurrentPowerUp == Enums.EPowerUp.Laser)
                    {
                        Fire();
                    }
                }
            }

            if (timer > 0)
                timer -= Time.deltaTime;
        }
    }
}