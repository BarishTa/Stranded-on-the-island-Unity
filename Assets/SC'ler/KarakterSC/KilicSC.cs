using UnityEngine;

public class KilicSC : MonoBehaviour
{
    // D��manlar�n can�n� azaltacak miktar
    public int azalacakCanMiktari = 50;

    // K�l�c�n collider'� bir �eye �arparsa bu fonksiyon �a�r�l�r
    private void OnTriggerEnter(Collider other)
    {
        // Bu nesnenin tag'� "Kilic" ise
        if (gameObject.CompareTag("Kilic"))
        {
            // �arp�lan nesne "Enemy" tag'ine sahipse
            if (other.CompareTag("Enemy"))
            {
                // �arp�lan nesnenin "SpiderSC" script'ini direkt olarak al
                SpiderSC spider = other.GetComponent<SpiderSC>();

                // E�er �arp�lan nesne bir �r�mcek ise ve �r�mcek script'i varsa
                if (spider != null)
                {
                    // �r�mce�in can�n� azalt
                    spider.AzalCan(azalacakCanMiktari);

                    // �r�mce�in yeni can�n� debug log ile g�ster
                    Debug.Log("�r�mce�in can� azald�! Yeni can: " + spider.orumcekCan�);
                }
            }
            // �arp�lan nesne "Boss" tag'ine sahipse
            else if (other.CompareTag("Boss"))
            {
                // �arp�lan nesnenin "BossSc" script'ini direkt olarak al
                BossSc boss = other.GetComponent<BossSc>();

                // E�er �arp�lan nesne bir boss ise ve boss script'i varsa
                if (boss != null)
                {
                    // Boss'un can�n� azalt
                    boss.AzalCan(azalacakCanMiktari);

                    // Boss'un yeni can�n� debug log ile g�ster
                    Debug.Log("Boss'un can� azald�! Yeni can: " + boss.bossCan�);
                }
            }
        }
    }
}
