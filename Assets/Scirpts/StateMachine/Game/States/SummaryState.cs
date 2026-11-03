using System.Collections;
using UITransition.Enums;
using UnityEngine;


namespace StateMachine.Game.States
{
    public class SummaryState : GameState
    {
        protected override IEnumerator Enter()
        {
            GameManager.Instance.UIManager.OpenSummaryPanel();
            return base.Enter();
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Cover);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenCovered);
            GameManager.Instance.UIManager.CloseSummaryPanel();
        }
    }
}