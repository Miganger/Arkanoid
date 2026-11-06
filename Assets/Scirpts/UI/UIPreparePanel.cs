using UnityEngine;
using TMPro;

public class UIPreparePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;

    public void Open(int round)
    {
        roundText.text = round.ToString();
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
