using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    public List<EndLevel> EndLevelTiles = new List<EndLevel>();
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Turrets = new List<GameObject>();
    public List<CollectCoin> Coins = new List<CollectCoin>();
    public Player Player;

    public void DestroyGameObjects()
    {
        MonoBehaviour.Destroy(Player.gameObject);

        foreach (GameObject enemy in Enemies)
        {
            if (enemy != null)
            { 
                MonoBehaviour.Destroy(enemy);
            }
        }

        foreach (GameObject turret in Turrets)
        {
            MonoBehaviour.Destroy(turret);
        }

        foreach (CollectCoin coin in Coins)
        {
            if (coin != null)
            { 
                MonoBehaviour.Destroy(coin.gameObject);
            }
        }

        foreach (EndLevel tile in EndLevelTiles)
        {
            MonoBehaviour.Destroy(tile.gameObject);
        }
    }
}