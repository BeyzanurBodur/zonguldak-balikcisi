using UnityEngine;

public class KancaKontrol : MonoBehaviour
{
    public BalikciKontrol balikciKodu;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. DURUM: NORMAL BALIK (Hamsi)
        if (other.CompareTag("Fish") && balikciKodu.guncelDurum == BalikciKontrol.Durum.Firlatiliyor)
        {
            if (other.GetComponent<BalikHareket>() != null)
                other.GetComponent<BalikHareket>().enabled = false;

            // Normal hýz (Kendi BalikciKontrol kodundaki varsayýlan hýz)
            balikciKodu.cekmeHizi = 10f;
            balikciKodu.BalikYakalandi(other.transform);
        }

        // 2. DURUM: BÜYÜK BALIK (PALAMUT - Yavaþ çekilir)
        else if (other.CompareTag("BuyukBalik") && balikciKodu.guncelDurum == BalikciKontrol.Durum.Firlatiliyor)
        {
            if (other.GetComponent<BalikHareket>() != null)
                other.GetComponent<BalikHareket>().enabled = false;

            // AÐIR ÇEKÝM HIZI (Çok yavaþ gelir, oyuncu zorluðu hissetsin)
            balikciKodu.cekmeHizi = 3f;
            balikciKodu.BalikYakalandi(other.transform);
        }

        // 3. DURUM: YOSUN
        else if (other.CompareTag("Yosun") && balikciKodu.guncelDurum == BalikciKontrol.Durum.Firlatiliyor)
        {
            LevelManager.instance.YosunaCarpildi(5f);

            if (other.GetComponent<YosunHareket>() != null)
                other.GetComponent<YosunHareket>().enabled = false;

            balikciKodu.cekmeHizi = 10f; // Yosun normal hýzda gelsin (veya onu da yavaþlatabilirsin)
            balikciKodu.BalikYakalandi(other.transform);
        }
    }
}