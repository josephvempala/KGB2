using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static readonly Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) Destroy(this);
    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
    {
        var player = Instantiate(id == Client.Instance.id ? localPlayerPrefab : playerPrefab, position, rotation);
        var playerComponent = player.GetComponent<PlayerManager>();
        playerComponent.id = id;
        playerComponent.username = username;
        Players.Add(id, playerComponent);
    }
}