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
            GameManager.Instance.AudioController.PlaySoundEffect(Enums.Audio.ESoundType.NewLevel);

            GameManager.Instance.LevelsManager.GenerateLevel(GameManager.Instance.CurrentLevelID);

            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Reveal);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenRevealed);

            GameManager.Instance.UIManager.PreparePanel.Open(GameManager.Instance.CurrentLevelID);
            yield return new WaitForSeconds(3);
            ChangeState(EGameState.Gameplay);
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.UIManager.PreparePanel.Close();
            return base.Exit();
        }
    }
}
