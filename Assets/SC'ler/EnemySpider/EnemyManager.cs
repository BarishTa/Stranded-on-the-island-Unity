using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject boss; // Sahneye yerle�tirilmi� deaktif Boss GameObject'i
    private bool bossSpawned = false; // Boss'un spawn olup olmad���n� kontrol eden de�i�ken

    void Start()
    {
        if (boss != null)
        {
            boss.SetActive(false); // Ba�lang��ta Boss'u deaktif yap
        }
        else
        {
            Debug.LogError("Boss GameObject not assigned in EnemyManager!");
        }
    }

    void Update()
    {
        if (!bossSpawned)
        {
            CheckAllEnemiesDead();
        }
    }

    public void CheckAllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            ActivateBoss();
        }
    }

    private void ActivateBoss()
    {
        if (boss != null)
        {
            boss.SetActive(true); // Boss'u aktif hale getir
            bossSpawned = true; // Boss aktif oldu, kontrol� durdur
        }
    }
}
