using System.Collections.Generic;
using UnityEngine;

public class LeaderboardBootstrap : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject itemPrefab;

    [Header("Mock Data")]
    [SerializeField] private string myPlayerId = "player_001";

    private void Start()
    {
        List<PlayerData> players = GenerateMockPlayers();

        players.Sort((a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];

            GameObject itemGO = Instantiate(itemPrefab, contentTransform);
            var item = itemGO.GetComponent<LeaderboardItem>();
            item.Initialize(i + 1, player.Name, player.Score);

            if (player.Id == myPlayerId)
            {
                item.SetIsMyPlayer(true);
            }
        }
    }
    
    private List<PlayerData> GenerateMockPlayers()
    {
        return new List<PlayerData>
        {
            new PlayerData("player_001", "Winnipeg", 116),
            new PlayerData("player_002", "Tampa Bay", 102),
            new PlayerData("player_003", "Florida", 98),
            new PlayerData("player_004", "Ottawa", 97),
            new PlayerData("player_005", "Montreal", 91),
            new PlayerData("player_006", "Detroit", 86),
            new PlayerData("player_007", "Buffalo", 79),
            new PlayerData("player_008", "Boston", 76),
            new PlayerData("player_009", "Washington", 111),
            new PlayerData("player_010", "Carolina", 99),
            new PlayerData("player_011", "New Jersey", 91),
            new PlayerData("player_012", "Columbus", 89),
            new PlayerData("player_013", "Rangers", 85),
            new PlayerData("player_014", "Islanders", 82),
            new PlayerData("player_015", "Pittsburgh", 80),
            new PlayerData("player_016", "Flyers", 76),
            new PlayerData("player_017", "Toronto", 108),
            new PlayerData("player_018", "Dallas", 106),
            new PlayerData("player_019", "Colorado", 102),
            new PlayerData("player_020", "Minnesota", 97),
        };
    }
}

[System.Serializable]
public class PlayerData
{
    public string Id;
    public string Name;
    public int Score;

    public PlayerData(string id, string name, int score)
    {
        Id = id;
        Name = name;
        Score = score;
    }
}
