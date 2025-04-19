using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    public static event System.Action<LeaderboardItem> OnMyPlayerChanged;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Sprite defaultBackground;
    [SerializeField] private Sprite myBackground;

    [SerializeField] private bool isMyPlayer = false;

    public void Initialize(int position, string playerName, int score)
    {
        idText.text = position.ToString();
        nameText.text = playerName;
        scoreText.text = score.ToString();
        
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        GetComponent<Image>().sprite = isMyPlayer ? myBackground : defaultBackground;
    }

    public void SetIsMyPlayer(bool value)
    {
        isMyPlayer = value;
        UpdateVisual();
        if (isMyPlayer)
        {
            OnMyPlayerChanged?.Invoke(this);
        }
    }

    public bool IsMyPlayer => isMyPlayer;
}
