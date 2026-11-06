using System.Collections;
using UITransition.Enums;
using UnityEngine;


namespace StateMachine.Game.States
{
    public class SummaryState : GameState
    {
        protected override IEnumerator Enter()
        {
            if (!GameManager.Instance.IsWin)
                GameManager.Instance.AudioController.PlaySoundEffect(Enums.Audio.ESoundType.Fail);

            GameManager.Instance.UIManager.SummaryPanel.Open(
                GameManager.Instance.IsWin,
                GameManager.Instance.ScoreManager.Score,
                GameManager.Instance.ScoreManager.BestScore);
            return base.Enter();
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Cover);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenCovered);
            GameManager.Instance.UIManager.SummaryPanel.Close();
        }
    }
}