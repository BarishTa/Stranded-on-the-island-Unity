using System.Collections;
using UnityEngine;

public class DrgScript : MonoBehaviour
{
    public float followRadius = 10f;
    public float attackRadius = 3f;
    public int damageAmount = 20; // Oyuncunun alacaðý hasar
    public float attackInterval = 2f; // Saldýrý aralýðý
    public int drgCaný = 100; // Düþmanýn caný
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
            if (playerHealth != null && playerHealth.karakterCaný > 0)
            {
                playerHealth.AzalCan(damageAmount);
                Debug.Log("Karakterin caný azaldý! Yeni can: " + playerHealth.karakterCaný);

                if (playerHealth.karakterCaný <= 0)
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
        drgCaný -= miktar;
        if (drgCaný <= 0 && !isDead)
        {
            Debug.Log("Ejderha öldü!");
            isDead = true;
            drgAnim.SetDie(true);
            // Diðer ölüm iþlemleri buraya eklenebilir
            // Örneðin, düþman yok etme veya yeniden doðurma
        }
    }
}
