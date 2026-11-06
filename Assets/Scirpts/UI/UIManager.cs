using System;
using UITransition;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public UITransitionController TransitionController => transitionController;
        public UIPreparePanel PreparePanel => preparePanel;
        public UISummaryPanel SummaryPanel => summaryPanel;

        [Header("Controllers References")]
        [SerializeField] private UITransitionController transitionController;
        [SerializeField] private UILifes lifes;

        [Header("Panels References")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private UIPreparePanel preparePanel;
        [SerializeField] private UISummaryPanel summaryPanel;
        [SerializeField] private UISettingsPanel settingsPanel;

        public void OpenSettingsPanel()
        {
            settingsPanel.Open();
        }

        public void Setup()
        {
            lifes.Setup();
        }

        public void OpenMainMenu()
        {
            mainMenuPanel.SetActive(true);
        }
        public void CloseMainMenu()
        {
            mainMenuPanel.SetActive(false);
        }
    }
}
