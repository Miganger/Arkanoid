
using System.Collections;
using UnityEngine;

namespace StateMachine.Game.States
{
    public class GameplayState : GameState
    {
        protected override IEnumerator Enter()
        {
            GameManager.Instance.PaddleController.DisableActions();
            GameManager.Instance.PaddleController.EnablePaddle();
            yield return new WaitForSeconds(1);
            GameManager.Instance.PaddleController.EnableActions();
            GameManager.Instance.BallsController.SpawnBall();
            yield return new WaitForSeconds(1);
            GameManager.Instance.EnemiesManager.StartSpawning();
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.PowerUpsManager.ClearPowerUps();


            GameManager.Instance.EnemiesManager.StopSpawning();

            GameManager.Instance.BallsController.ClearBalls();
            GameManager.Instance.LevelsManager.ClearLevel();

            GameManager.Instance.PaddleController.DisablePaddle();
            GameManager.Instance.PaddleController.DisableActions();

            return base.Exit();
        }
    }
}