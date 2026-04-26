using UnityEngine;
using TMPro;

public class SkorManager : MonoBehaviour
{
    public static SkorManager instance;
    public TextMeshProUGUI anaSkorYazisi;
    public TextMeshProUGUI bildirimYazisi;

    private int toplamSkor = 0;

    void Awake() { instance = this; }

    public void BalikTutuldu(string balikAdi, int puan)
    {
        toplamSkor += puan;
        if (anaSkorYazisi != null) anaSkorYazisi.text = "Toplam: " + toplamSkor;
        // Bildirim kýsmýný sonra aktif edebilirsin
    }
}