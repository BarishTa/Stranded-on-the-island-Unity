using UnityEngine;

public class WrAnimation : MonoBehaviour
{
    private Animator animator; // Animator bile�eni
    private bool isWalking = false; // Y�r�me durumu
    private bool isDead = false; // �l�m durumu

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator bile�enini al
    }

    void Update()
    {
        // "isWalking" ve "isDead" de�erine g�re animasyonu oynat veya durdur
        if (isDead)
        {
            animator.SetBool("Die", true);
        }
        else
        {
            animator.SetBool("Walk", isWalking);
        }
    }

    // Y�r�me animasyonunu ba�latan fonksiyon
    public void StartWalkingAnimation()
    {
        if (!isDead)
        {
            isWalking = true;
        }
    }

    // Y�r�me animasyonunu durduran fonksiyon
    public void StopWalkingAnimation()
    {
        if (!isDead)
        {
            isWalking = false;
        }
    }

    // �l�m animasyonunu ba�latan fonksiyon
    public void Die()
    {
        isDead = true;
        isWalking = false;
        animator.SetBool("Die", true);
    }
}
