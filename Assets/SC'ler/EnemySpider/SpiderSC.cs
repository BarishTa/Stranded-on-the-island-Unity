using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSC : MonoBehaviour
{
    public GameObject kilicPrefab; // Kilic prefab'�
    public float detectionRadius = 11.29f; // �r�mce�in alg�lama yar��ap�
    public float attackRadius = 4.84f; // Sald�r� yar��ap�
    public float moveSpeed = 3f; // �r�mce�in hareket h�z�
    public int orumcekCan� = 100; // �r�mce�in can�
    public int karaktereAzalacakCanMiktari = 35; // Karakterin can�n� azaltacak miktar
    public float attackInterval = 1f; // Sald�r� aral��� (saniye cinsinden)

    private Transform playerTransform;
    private SpiderAnim spiderAnim;
    private bool isDead = false;
    private bool isAttacking = false; // Sald�r� durumu
    private WrScripts character; // Karakterin script referans�

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            character = player.GetComponent<WrScripts>();
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        spiderAnim = GetComponent<SpiderAnim>();
        if (spiderAnim == null)
        {
            Debug.LogError("SpiderAnim component missing on Spider!");
        }
    }

    void Update()
    {
        if (!isDead && playerTransform != null && orumcekCan� > 0) // �r�mcek �lmediyse hareket et
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius)
            {
                StartWalking();
            }
            else if (distanceToPlayer <= attackRadius)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayerContinuously());
                }
            }
            else
            {
                StopActions();
            }
        }
    }

    private void StartWalking()
    {
        spiderAnim.StartWalking();
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        Vector3 lookAtPosition = playerTransform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
        transform.Rotate(0, 180, 0);
    }

    private IEnumerator AttackPlayerContinuously()
    {
        isAttacking = true;
        spiderAnim.StartAttacking();

        while (Vector3.Distance(transform.position, playerTransform.position) <= attackRadius && !isDead)
        {
            if (character != null && character.karakterCan� > 0)
            {
                character.karakterCan� -= karaktereAzalacakCanMiktari;
                Debug.Log("Karakterin can� azald�! Yeni can: " + character.karakterCan�);
            }
            yield return new WaitForSeconds(attackInterval); // Sald�r� aral��� kadar bekle
        }

        isAttacking = false;
        spiderAnim.StopAttacking();
    }

    private void StopActions()
    {
        spiderAnim.StopWalking();
        spiderAnim.StopAttacking();
        isAttacking = false; // Sald�r�y� durdur
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDead)
        {
            WrScripts character = collision.gameObject.GetComponent<WrScripts>();
            if (character != null)
            {
                StartCoroutine(DamageCharacterOverTime(character));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isDead)
        {
            WrScripts character = collision.gameObject.GetComponent<WrScripts>();
            if (character != null)
            {
                StopCoroutine(DamageCharacterOverTime(character));
            }
        }
    }

    private IEnumerator DamageCharacterOverTime(WrScripts character)
    {
        while (!isDead && character != null && character.karakterCan� > 0)
        {
            character.karakterCan� -= karaktereAzalacakCanMiktari;
            Debug.Log("Karakterin can� azald�! Yeni can: " + character.karakterCan�);
            yield return new WaitForSeconds(attackInterval); // Sald�r� aral��� kadar bekle
        }
    }

    public void AzalCan(int miktar)
    {
        orumcekCan� -= miktar;
        if (orumcekCan� <= 0 && !isDead)
        {
            Debug.Log("�r�mcek �ld�!");
            isDead = true;
            SpawnSword();
            spiderAnim.StartDying();
            StopActions();
            NotifyEnemyManager(); // EnemyManager'a d��man�n �ld���n� bildir
        }
    }

    private void NotifyEnemyManager()
    {
        EnemyManager enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.CheckAllEnemiesDead();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void ResumeFollowing()
    {
        if (!isDead)
        {
            StartWalking();
        }
    }

    private void SpawnSword()
    {
        if (kilicPrefab != null)
        {
            GameObject kilic = Instantiate(kilicPrefab, transform.position, Quaternion.identity);
            kilic.tag = "Kilic";
        }
        else
        {
            Debug.LogError("Kilic prefab not assigned!");
        }
    }
}
