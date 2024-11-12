using System.Collections;
using UnityEngine;

public class DrgScript : MonoBehaviour
{
    public float followRadius = 10f;
    public float attackRadius = 3f;
    public int damageAmount = 20; // Oyuncunun alaca�� hasar
    public float attackInterval = 2f; // Sald�r� aral���
    public int drgCan� = 100; // D��man�n can�
    private Transform playerTransform;
    private DrgAnim drgAnim;
    private bool isAttacking = false;
    private WrScripts playerHealth;
    private bool isDead = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<WrScripts>();
            if (playerHealth == null)
            {
                Debug.LogError("WrScripts component not found on Player!");
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        drgAnim = GetComponent<DrgAnim>();
        if (drgAnim == null)
        {
            Debug.LogError("DrgAnim component missing on Enemy!");
        }
    }

    void Update()
    {
        if (isDead) return;

        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRadius)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayerContinuously());
                }
            }
            else if (distanceToPlayer <= followRadius)
            {
                StopAllCoroutines();
                isAttacking = false;
                FollowPlayer();
            }
            else
            {
                drgAnim.SetWalk(false);
                StopAllCoroutines();
                isAttacking = false;
            }
        }
    }

    private void FollowPlayer()
    {
        drgAnim.SetWalk(true);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Time.deltaTime);
        Vector3 lookAtPosition = playerTransform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    private IEnumerator AttackPlayerContinuously()
    {
        isAttacking = true;
        drgAnim.SetWalk(false);
        drgAnim.TriggerAttack();

        while (Vector3.Distance(transform.position, playerTransform.position) <= attackRadius)
        {
            if (playerHealth != null && playerHealth.karakterCan� > 0)
            {
                playerHealth.AzalCan(damageAmount);
                Debug.Log("Karakterin can� azald�! Yeni can: " + playerHealth.karakterCan�);

                if (playerHealth.karakterCan� <= 0)
                {
                    playerHealth.Die();
                    break;
                }
            }
            yield return new WaitForSeconds(attackInterval);
        }

        isAttacking = false;
    }

    public void AzalCan(int miktar)
    {
        drgCan� -= miktar;
        if (drgCan� <= 0 && !isDead)
        {
            Debug.Log("Ejderha �ld�!");
            isDead = true;
            drgAnim.SetDie(true);
            // Di�er �l�m i�lemleri buraya eklenebilir
            // �rne�in, d��man yok etme veya yeniden do�urma
        }
    }
}
