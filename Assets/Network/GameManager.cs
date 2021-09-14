using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void SpawnPlayer(int id, string username, Vector3 postion, Quaternion rotation)
    {
        GameObject player;
        if(id == Client.instance.id)
        {
            player = Instantiate(localPlayerPrefab, postion, rotation);
        }
        else
        {
            player = Instantiate(playerPrefab, postion, rotation);
        }
        var playerComponent = player.GetComponent<PlayerManager>();
        playerComponent.id = id;
        playerComponent.username = username;
        players.Add(id, playerComponent);
    }
    
}
