using UnityEngine;

public class KilicAlma : MonoBehaviour
{
    private GameObject player;
    private GameObject currentSword;

    private void Start()
    {
        // Oyun ba�lad���nda Player objesini bul
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Kilic") && Input.GetKeyDown(KeyCode.E))
        {
            // E tu�una bas�ld���nda ve Kilic tag'li bir nesneye temas edildi�inde
            if (player != null)
            {
                // E�er Player'�n elinde zaten bir k�l�� varsa, onu yok et
                if (currentSword != null)
                {
                    Destroy(currentSword);
                }

                // Kilic tag'li nesneyi Player'�n eline al
                other.transform.parent = player.transform;
                other.transform.localPosition = Vector3.zero;
                other.transform.localRotation = Quaternion.identity;

                // Yerden al�nan k�l�c� kaydet
                currentSword = other.gameObject;
            }
        }
    }
}
