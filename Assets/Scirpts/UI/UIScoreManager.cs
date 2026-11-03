using UnityEngine;
using TMPro;
using System;

namespace UI
{
    public class UIScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreValueText;
        [SerializeField] private TextMeshProUGUI bestScoreValueText;

        private void OnEnable()
        {
            ScoreManager.OnScoreUpdated += OnScoreUpdated;
            ScoreManager.OnBestScoreUpdated += OnBestScoreUpdated;
        }
        private void OnDisable()
        {
            ScoreManager.OnScoreUpdated -= OnScoreUpdated;
            ScoreManager.OnBestScoreUpdated -= OnBestScoreUpdated;
        }

        private void OnBestScoreUpdated(uint bestScore)
        {
            bestScoreValueText.text = bestScore.ToString();
        }

        public void OnScoreUpdated(uint score)
        {
            scoreValueText.text = score.ToString();
        }
    }
}
