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
    public int bossCaný = 50;
    public float despawnTime = 5f;
    public bool BossAttack { get; private set; } = false;
    public bool BossWalk { get; private set; } = false;
    public bool BossDie { get; private set; } = false;
    public int karaktereAzalacakCanMiktari = 100; // Karakterin alacaðý hasar miktarý
    public float attackInterval = 1f; // Saldýrý aralýðý

    private Transform playerTransform;
    private WrScripts character; // WrScripts referansý
    private BossAnim bossAnim;
    private bool isDead = false;
    private bool isAttacking = false; // Saldýrýyor mu durumu

    public GameObject successScreenCanvas; // Baþarý ekraný Canvas
    public Button nextLevelButton; // Sýradaki Bölüm butonu

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            character = player.GetComponent<WrScripts>(); // WrScripts scriptini alýn
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
            successScreenCanvas.SetActive(false); // Baþlangýçta baþarý ekraný devre dýþý
        }
        else
        {
            Debug.LogError("Success Screen Canvas bulunamadý!");
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked); // Butona týklama olayýný ekle
        }
        else
        {
            Debug.LogError("Next Level Button bulunamadý!");
        }
    }

    void Update()
    {
        if (!isDead && playerTransform != null && bossCaný > 0)
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
            if (character != null && character.karakterCaný > 0)
            {
                character.karakterCaný -= karaktereAzalacakCanMiktari;
                Debug.Log("Karakterin caný azaldý! Yeni can: " + character.karakterCaný);

                if (character.karakterCaný <= 0)
                {
                    character.Die();
                    break;
                }
            }
            yield return new WaitForSeconds(attackInterval); // Saldýrý aralýðý kadar bekle
        }

        isAttacking = false;
        bossAnim.StopAttacking();
    }

    public void AzalCan(int miktar)
    {
        bossCaný -= miktar;
        if (bossCaný <= 0 && !isDead)
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
            successScreenCanvas.SetActive(true); // Baþarý ekranýný göster
            Cursor.lockState = CursorLockMode.None; // Fare iþaretçisini serbest býrak
            Cursor.visible = true; // Fare iþaretçisini görünür yap
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
            // Player karakterini bul ve sahneyi yükle
            SceneManager.LoadScene("2.sahne");

            // Yeni sahne yüklendikten sonra karakteri doðru spawn noktasýna taþý
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne yüklendiðinde bu fonksiyonu çaðýr

            // Player karakterini yok etmeden taþý
            DontDestroyOnLoad(player);
        }
        else
        {
            Debug.LogError("Player karakteri bulunamadý!");
        }
    }

    // Sahne yüklendikten sonra çaðrýlacak fonksiyon
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
                Debug.LogError("Player veya spawn noktasý bulunamadý!");
            }
        }

        // Sahne yükleme olayýný temizle
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
