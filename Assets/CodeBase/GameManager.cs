using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public NetworkList<ScoreEntry> scoreBoard;

    public Transform scorePanel; // UI panel with layout group
    public GameObject scoreRowPrefab; // prefab with TMP_Texts: Name, Kills, Deaths

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
            scoreBoard = new NetworkList<ScoreEntry>();

        if (IsClient)
            scoreBoard.OnListChanged += UpdateUI;
    }

    public void RegisterPlayer(ulong clientId, string name)
    {
        if (!IsServer) return;

        bool exists = false;
        foreach (var entry in scoreBoard)
        {
            if (entry.ClientId == clientId)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            scoreBoard.Add(new ScoreEntry
            {
                PlayerName = name,
                ClientId = clientId,
                Kills = 0,
                Deaths = 0
            });
        }
    }

    public void AddKill(ulong killerId, ulong victimId)
    {
        if (!IsServer) return;

        for (int i = 0; i < scoreBoard.Count; i++)
        {
            if (scoreBoard[i].ClientId == killerId)
            {
                var entry = scoreBoard[i];
                entry.Kills++;
                scoreBoard[i] = entry;
            }
            if (scoreBoard[i].ClientId == victimId)
            {
                var entry = scoreBoard[i];
                entry.Deaths++;
                scoreBoard[i] = entry;
            }
        }
    }

    private void UpdateUI(NetworkListEvent<ScoreEntry> change)
    {
        foreach (Transform child in scorePanel)
            Destroy(child.gameObject);

        foreach (var entry in scoreBoard)
        {
            var row = Instantiate(scoreRowPrefab, scorePanel);
            var texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = entry.PlayerName.ToString();
            texts[1].text = entry.Kills.ToString();
            texts[2].text = entry.Deaths.ToString();
        }
    }
}