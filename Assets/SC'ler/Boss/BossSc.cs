using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossSc : MonoBehaviour
{
    public GameObject bossPrefab;
    public float attackRadius = 7f;
    public float moveSpeed = 3f;
    public int bossCan� = 50;
    public float despawnTime = 5f;
    public bool BossAttack { get; private set; } = false;
    public bool BossWalk { get; private set; } = false;
    public bool BossDie { get; private set; } = false;
    public int karaktereAzalacakCanMiktari = 100; // Karakterin alaca�� hasar miktar�
    public float attackInterval = 1f; // Sald�r� aral���

    private Transform playerTransform;
    private WrScripts character; // WrScripts referans�
    private BossAnim bossAnim;
    private bool isDead = false;
    private bool isAttacking = false; // Sald�r�yor mu durumu

    public GameObject successScreenCanvas; // Ba�ar� ekran� Canvas
    public Button nextLevelButton; // S�radaki B�l�m butonu

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            character = player.GetComponent<WrScripts>(); // WrScripts scriptini al�n
            if (character == null)
            {
                Debug.LogError("WrScripts component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        bossAnim = GetComponent<BossAnim>();
        if (bossAnim == null)
        {
            Debug.LogError("BossAnim component missing on Boss!");
        }

        if (successScreenCanvas != null)
        {
            successScreenCanvas.SetActive(false); // Ba�lang��ta ba�ar� ekran� devre d���
        }
        else
        {
            Debug.LogError("Success Screen Canvas bulunamad�!");
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked); // Butona t�klama olay�n� ekle
        }
        else
        {
            Debug.LogError("Next Level Button bulunamad�!");
        }
    }

    void Update()
    {
        if (!isDead && playerTransform != null && bossCan� > 0)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRadius)
            {
                BossAttack = true;
                BossWalk = false;
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayerContinuously());
                }
            }
            else
            {
                BossWalk = true;
                BossAttack = false;
                StopCoroutine(AttackPlayerContinuously());
                StartWalking();
            }
        }
    }

    private void StartWalking()
    {
        bossAnim.StartWalking();
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        Vector3 lookAtPosition = playerTransform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    private IEnumerator AttackPlayerContinuously()
    {
        isAttacking = true;
        bossAnim.StartAttacking();

        while (Vector3.Distance(transform.position, playerTransform.position) <= attackRadius && !isDead)
        {
            if (character != null && character.karakterCan� > 0)
            {
                character.karakterCan� -= karaktereAzalacakCanMiktari;
                Debug.Log("Karakterin can� azald�! Yeni can: " + character.karakterCan�);

                if (character.karakterCan� <= 0)
                {
                    character.Die();
                    break;
                }
            }
            yield return new WaitForSeconds(attackInterval); // Sald�r� aral��� kadar bekle
        }

        isAttacking = false;
        bossAnim.StopAttacking();
    }

    public void AzalCan(int miktar)
    {
        bossCan� -= miktar;
        if (bossCan� <= 0 && !isDead)
        {
            isDead = true;
            BossDie = true;
            bossAnim.StartDying();
            StartCoroutine(DespawnBoss());
        }
    }

    private IEnumerator DespawnBoss()
    {
        yield return new WaitForSeconds(despawnTime);

        if (successScreenCanvas != null)
        {
            successScreenCanvas.SetActive(true); // Ba�ar� ekran�n� g�ster
            Cursor.lockState = CursorLockMode.None; // Fare i�aret�isini serbest b�rak
            Cursor.visible = true; // Fare i�aret�isini g�r�n�r yap
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void OnNextLevelButtonClicked()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Player karakterini bul ve sahneyi y�kle
            SceneManager.LoadScene("2.sahne");

            // Yeni sahne y�klendikten sonra karakteri do�ru spawn noktas�na ta��
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne y�klendi�inde bu fonksiyonu �a��r

            // Player karakterini yok etmeden ta��
            DontDestroyOnLoad(player);
        }
        else
        {
            Debug.LogError("Player karakteri bulunamad�!");
        }
    }

    // Sahne y�klendikten sonra �a�r�lacak fonksiyon
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "2.sahne")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("Nokta");

            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
            }
            else
            {
                Debug.LogError("Player veya spawn noktas� bulunamad�!");
            }
        }

        // Sahne y�kleme olay�n� temizle
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
