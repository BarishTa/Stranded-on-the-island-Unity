using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Oyuncu prefab'ý
    public Transform spawnPoint; // Spawn noktasý

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // Oyuncu prefab'ýný spawn et
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            // Mevcut oyuncuyu spawn noktasýna taþý
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            Cursor.visible = false;
        }
    }
}
