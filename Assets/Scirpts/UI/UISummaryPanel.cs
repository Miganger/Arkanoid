using UnityEngine;
using TMPro;

namespace UI
{
    public class UISummaryPanel : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject loosePanel;
        [SerializeField] private GameObject scorePanel;
        [SerializeField] private TextMeshProUGUI bestScoreText;

        public void Open(bool win, uint currentScore, uint bestScore)
        {
            winPanel.SetActive(win);
            loosePanel.SetActive(!win);

            scorePanel.SetActive(currentScore == bestScore);
            bestScoreText.text = bestScore.ToString();

            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
