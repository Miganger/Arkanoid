using System.Collections;
using UnityEngine;
using Enums;
using UITransition.Enums;

namespace StateMachine.Game.States
{
    public class PrepareState : GameState
    {
        protected override IEnumerator Enter()
        {
            GameManager.Instance.LevelsManager.GenerateLevel(GameManager.Instance.CurrentLevelID);
            GameManager.Instance.ScoreManager.RestartScore();

            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Reveal);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenRevealed);

            GameManager.Instance.UIManager.OpenPreparePanel();
            yield return new WaitForSeconds(3);
            ChangeState(EGameState.Gameplay);
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.UIManager.ClosePreparePanel();
            return base.Exit();
        }
    }
}
