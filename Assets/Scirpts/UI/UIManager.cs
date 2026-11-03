using UITransition;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public UITransitionController TransitionController => transitionController;

        [Header("Controllers References")]
        [SerializeField] private UITransitionController transitionController;
        [SerializeField] private UILifes lifes;

        [Header("Panels References")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject preparePanel;
        [SerializeField] private GameObject summaryPanel;

        public void Setup()
        {
            lifes.Setup();
        }

        public void OpenSummaryPanel()
        {
            summaryPanel.SetActive(true);
        }
        public void CloseSummaryPanel()
        {
            summaryPanel.SetActive(false);
        }

        public void OpenMainMenu()
        {
            mainMenuPanel.SetActive(true);
        }
        public void CloseMainMenu()
        {
            mainMenuPanel.SetActive(false);
        }

        public void OpenPreparePanel()
        {
            preparePanel.SetActive(true);
        }
        public void ClosePreparePanel()
        {
            preparePanel.SetActive(false);
        }
    }
}
