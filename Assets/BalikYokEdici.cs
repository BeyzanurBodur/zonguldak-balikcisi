using UnityEngine;

public class BalikYokEdici : MonoBehaviour
{
    // Bu görünmez duvara bir þey çarptýðýnda çalýþýr (Is Trigger seçili olmalý)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpan þey balýk veya yosun ise onu acýmadan yok et
        if (other.CompareTag("Fish") || other.CompareTag("BuyukBalik") || other.CompareTag("Yosun"))
        {
            Destroy(other.gameObject);
        }
    }
}