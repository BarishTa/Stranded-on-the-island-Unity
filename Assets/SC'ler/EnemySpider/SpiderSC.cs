using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSC : MonoBehaviour
{
    public GameObject kilicPrefab; // Kilic prefab'ý
    public float detectionRadius = 11.29f; // Örümceðin algýlama yarýçapý
    public float attackRadius = 4.84f; // Saldýrý yarýçapý
    public float moveSpeed = 3f; // Örümceðin hareket hýzý
    public int orumcekCaný = 100; // Örümceðin caný
    public int karaktereAzalacakCanMiktari = 35; // Karakterin canýný azaltacak miktar
    public float attackInterval = 1f; // Saldýrý aralýðý (saniye cinsinden)

    private Transform playerTransform;
    private SpiderAnim spiderAnim;
    private bool isDead = false;
    private bool isAttacking = false; // Saldýrý durumu
    private WrScripts character; // Karakterin script referansý

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
        if (!isDead && playerTransform != null && orumcekCaný > 0) // Örümcek ölmediyse hareket et
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
            if (character != null && character.karakterCaný > 0)
            {
                character.karakterCaný -= karaktereAzalacakCanMiktari;
                Debug.Log("Karakterin caný azaldý! Yeni can: " + character.karakterCaný);
            }
            yield return new WaitForSeconds(attackInterval); // Saldýrý aralýðý kadar bekle
        }

        isAttacking = false;
        spiderAnim.StopAttacking();
    }

    private void StopActions()
    {
        spiderAnim.StopWalking();
        spiderAnim.StopAttacking();
        isAttacking = false; // Saldýrýyý durdur
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
        while (!isDead && character != null && character.karakterCaný > 0)
        {
            character.karakterCaný -= karaktereAzalacakCanMiktari;
            Debug.Log("Karakterin caný azaldý! Yeni can: " + character.karakterCaný);
            yield return new WaitForSeconds(attackInterval); // Saldýrý aralýðý kadar bekle
        }
    }

    public void AzalCan(int miktar)
    {
        orumcekCaný -= miktar;
        if (orumcekCaný <= 0 && !isDead)
        {
            Debug.Log("Örümcek öldü!");
            isDead = true;
            SpawnSword();
            spiderAnim.StartDying();
            StopActions();
            NotifyEnemyManager(); // EnemyManager'a düþmanýn öldüðünü bildir
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
