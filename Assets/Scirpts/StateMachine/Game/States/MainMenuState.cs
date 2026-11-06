using System.Collections;
using UITransition.Enums;
using UnityEngine;

namespace StateMachine.Game.States
{
    public class MainMenuState : GameState
    {
        protected override IEnumerator Enter()
        {
            GameManager.Instance.AudioController.PlayMusic(Enums.Audio.EMusicType.MainMenu);
            GameManager.Instance.UIManager.OpenMainMenu();
            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Reveal);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenRevealed);
        }

        protected override IEnumerator Exit()
        {
            GameManager.Instance.AudioController.StopMusic(Enums.Audio.EMusicType.MainMenu);
            GameManager.Instance.UIManager.TransitionController.PlayTransition(ETransition.Cover);
            yield return new WaitUntil(() => GameManager.Instance.UIManager.TransitionController.IsScreenCovered);
            GameManager.Instance.UIManager.CloseMainMenu();
            GameManager.Instance.ScoreManager.RestartScore();
            yield return new WaitForSeconds(1);
        }
    }
}