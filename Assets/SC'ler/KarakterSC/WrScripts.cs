using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI bileþenleri için

public class WrScripts : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6.0f;
    public int karakterCaný = 500;
    public Transform handPosition;
    public GameObject currentSword;

    bool isGrounded;
    bool isAttack;
    public bool isDead = false; // public olarak deðiþtirildi

    private Rigidbody rb;
    private WrAnimation wrAnimation;
    private AttackAnim attackAnim;
    private GameObject nearbySword;

    public GameObject deathScreenCanvas; // Ölüm ekraný Canvas

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wrAnimation = GetComponent<WrAnimation>();
        attackAnim = GetComponent<AttackAnim>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Baþlangýçta fare iþaretçisi gizli

        if (rb == null)
        {
            Debug.LogError("Rigidbody bileþeni bulunamadý!");
        }

        if (wrAnimation == null)
        {
            Debug.LogError("WrAnimation bileþeni bulunamadý!");
        }

        if (attackAnim == null)
        {
            Debug.LogError("AttackAnim bileþeni bulunamadý!");
        }

        isAttack = false;

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(false); // Baþlangýçta ölüm ekraný devre dýþý
        }
        else
        {
            Debug.LogError("Death Screen Canvas bulunamadý!");
        }
    }

    void Update()
    {
        if (karakterCaný <= 0 && !isDead)
        {
            Die();
        }

        if (isDead)
        {
            return;
        }

        if (!isAttack && !Input.GetMouseButton(0))
        {
            float verticalInput = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 movement = transform.forward * verticalInput;
                rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

                if (verticalInput != 0)
                {
                    wrAnimation.StartWalkingAnimation();
                }
                else
                {
                    wrAnimation.StopWalkingAnimation();
                }
            }
            else
            {
                wrAnimation.StopWalkingAnimation();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetMouseButton(0))
        {
            isAttack = true;
            wrAnimation.StopWalkingAnimation();
            attackAnim.PerformAttack();
        }
        else
        {
            isAttack = false;
        }

        if (nearbySword != null && Input.GetKeyDown(KeyCode.E))
        {
            PickupSword(nearbySword);
            Debug.Log("Colliderlar çarpýþýyor ve E tuþuna basýldý. Kýlýç yerden alýndý.");
        }
        else if (nearbySword == null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E tuþuna basýldý ancak yakýnda kýlýç yok.");
        }
    }

    public void Die()
    {
        isDead = true;
        wrAnimation.Die();

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(true); // Ölüm ekranýný göster
            Cursor.lockState = CursorLockMode.None; // Fare iþaretçisini serbest býrak
            Cursor.visible = true; // Fare iþaretçisini görünür yap
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked; // Fare iþaretçisini tekrar kilitle
        Cursor.visible = false; // Fare iþaretçisini gizle
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kilic"))
        {
            nearbySword = other.gameObject;
            Debug.Log("Kýlýç menzile girdi.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Kilic"))
        {
            nearbySword = null;
            Debug.Log("Kýlýç menzilden çýktý.");
        }
    }

    private void PickupSword(GameObject sword)
    {
        if (currentSword != null)
        {
            Destroy(currentSword);
            Debug.Log("Eski kýlýç yok edildi.");
        }

        currentSword = sword;
        sword.transform.SetParent(handPosition);
        sword.transform.localPosition = Vector3.zero;
        sword.transform.localRotation = Quaternion.identity;

        Collider swordCollider = sword.GetComponent<Collider>();
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }

    internal void AzalCan(int damageAmount)
    {
        throw new NotImplementedException();
    }
}
