using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Oyuncu prefab'�
    public Transform spawnPoint; // Spawn noktas�

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // Oyuncu prefab'�n� spawn et
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            // Mevcut oyuncuyu spawn noktas�na ta��
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            Cursor.visible = false;
        }
    }
}
