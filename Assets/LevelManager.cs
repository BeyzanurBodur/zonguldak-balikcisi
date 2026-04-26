using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("GİRİŞ EFEKTİ AYARLARI")]
    public Transform taka;
    public CanvasGroup arayuzGrubu;
    public float baslangicX = -15.78f;
    private bool arayuzAcildi = false;

    [Header("--- LEVEL HEDEFLERİ (BALIK ADEDİ) ---")]
    [Tooltip("Sırasıyla 1., 2. ve 3. levelin kaç balık istediğini buraya yazın")]
    public int[] hedefBalikSayilari = { 20, 30, 35 };

    [Header("Oyun İçi UI Bağlantıları")]
    public TextMeshProUGUI sureYazisi;
    public TextMeshProUGUI skorYazisi;
    public TextMeshProUGUI adetYazisi;
    public TextMeshProUGUI gorevAciklamaYazisi;
    public TextMeshProUGUI levelBaslikYazisi;

    [Header("LEVEL BİTİŞ PANELLERİ")]
    public GameObject level1BitisPaneli;
    public TextMeshProUGUI level1ParaRakam;
    public TextMeshProUGUI level1BalikRakam;
    public GameObject level2BitisPaneli;
    public TextMeshProUGUI level2ParaRakam;
    public TextMeshProUGUI level2BalikRakam;
    public GameObject level3BitisPaneli;
    public TextMeshProUGUI level3ParaRakam;
    public TextMeshProUGUI level3BalikRakam;

    [Header("KAYBETME PANELİ VE İSTATİSTİKLER")]
    public GameObject kaybetmePaneli;
    public TextMeshProUGUI paraKaybetmeYazisi;
    public TextMeshProUGUI balikKaybetmeYazisi;

    [Header("Oyun Ayarları")]
    public float kalanSure = 60f;
    public int hedefBalikSayisi = 10;
    public int suAnkiPuan = 0;
    public int suAnkiBalikAdedi = 0;
    public int gecerliLevel = 1;

    private bool oyunDurdu = false;

    void Awake() { instance = this; }

    void Start()
    {
        LeveliBaslat();
        if (arayuzGrubu != null) arayuzGrubu.alpha = 0;
    }

    void Update()
    {
        // --- YENİ EKLENEN KISIM: ESC İLE ÇIKIŞ ---
        // Oyun dursa bile ESC tuşu çalışsın diye en üste koyduk
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AnaMenuyeDon();
        }

        if (oyunDurdu) return;

        if (taka != null && taka.position.x < baslangicX) return;

        if (!arayuzAcildi)
        {
            StartCoroutine(ArayuzuYavascaAc());
            arayuzAcildi = true;
        }

        if (gecerliLevel != 4)
        {
            if (kalanSure > 0)
            {
                kalanSure -= Time.deltaTime;
                if (kalanSure < 0) kalanSure = 0;
                SureyiGuncelle();
            }
            else
            {
                // Sadece süre gerçekten biterse çalışır
                ZamanBittiKaybettin();
            }
        }
        else
        {
            if (sureYazisi != null) sureYazisi.text = "∞";
        }
    }

    IEnumerator ArayuzuYavascaAc()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.8f;
            if (arayuzGrubu != null) arayuzGrubu.alpha = t;
            yield return null;
        }
        if (arayuzGrubu != null) arayuzGrubu.alpha = 1f;
    }

    void LeveliBaslat()
    {
        if (level1BitisPaneli != null) level1BitisPaneli.SetActive(false);
        if (level2BitisPaneli != null) level2BitisPaneli.SetActive(false);
        if (level3BitisPaneli != null) level3BitisPaneli.SetActive(false);
        if (kaybetmePaneli != null) kaybetmePaneli.SetActive(false);

        Time.timeScale = 1;
        oyunDurdu = false;
        arayuzAcildi = false;
        suAnkiBalikAdedi = 0;

        if (gecerliLevel == 1)
        {
            kalanSure = 60f;
            hedefBalikSayisi = hedefBalikSayilari[0];
            levelBaslikYazisi.text = "LEVEL 1";
            gorevAciklamaYazisi.text = hedefBalikSayisi + " tane balık topla!";
        }
        else if (gecerliLevel == 2)
        {
            kalanSure = 120f;
            hedefBalikSayisi = hedefBalikSayilari[1];
            levelBaslikYazisi.text = "LEVEL 2";
            gorevAciklamaYazisi.text = "Dikkat yosunlar geldi, " + hedefBalikSayisi + " balık topla!";
        }
        else if (gecerliLevel == 3)
        {
            kalanSure = 120f;
            hedefBalikSayisi = hedefBalikSayilari[2];
            levelBaslikYazisi.text = "LEVEL 3";
            gorevAciklamaYazisi.text = "Palamutlar geldi, " + hedefBalikSayisi + " balık topla!";
        }
        else if (gecerliLevel == 4)
        {
            levelBaslikYazisi.text = "SONSUZ MOD";
            hedefBalikSayisi = 9999;
            gorevAciklamaYazisi.text = "Zonguldak Reisi! ESC ile ana menüye dönebilirsin!";
        }

        ArayuzuGuncelle();
    }

    public void BalikTutuldu(int gelenPara)
    {
        suAnkiPuan += gelenPara;
        suAnkiBalikAdedi++;
        ArayuzuGuncelle();

        // Sadece hedefi geçerse paneli aç, geçmediyse oyuna devam et.
        if (suAnkiBalikAdedi >= hedefBalikSayisi && gecerliLevel != 4)
        {
            PaneliGoster();
        }
    }

    void ZamanBittiKaybettin()
    {
        if (oyunDurdu) return; // Zaten durduysa bir daha çalıştırma

        oyunDurdu = true;
        Time.timeScale = 0;
        if (kaybetmePaneli != null) kaybetmePaneli.SetActive(true);

        if (paraKaybetmeYazisi != null) paraKaybetmeYazisi.text = suAnkiPuan.ToString() + " TL";
        if (balikKaybetmeYazisi != null) balikKaybetmeYazisi.text = suAnkiBalikAdedi.ToString() + " / " + hedefBalikSayisi.ToString();
    }

    void PaneliGoster()
    {
        oyunDurdu = true;
        Time.timeScale = 0;

        if (gecerliLevel == 1)
        {
            if (level1BitisPaneli != null) level1BitisPaneli.SetActive(true);
            if (level1ParaRakam != null) level1ParaRakam.text = suAnkiPuan.ToString() + " TL";
            if (level1BalikRakam != null) level1BalikRakam.text = suAnkiBalikAdedi.ToString();
        }
        else if (gecerliLevel == 2)
        {
            if (level2BitisPaneli != null) level2BitisPaneli.SetActive(true);
            if (level2ParaRakam != null) level2ParaRakam.text = suAnkiPuan.ToString() + " TL";
            if (level2BalikRakam != null) level2BalikRakam.text = suAnkiBalikAdedi.ToString();
        }
        else if (gecerliLevel == 3)
        {
            if (level3BitisPaneli != null) level3BitisPaneli.SetActive(true);
            if (level3ParaRakam != null) level3ParaRakam.text = suAnkiPuan.ToString() + " TL";
            if (level3BalikRakam != null) level3BalikRakam.text = suAnkiBalikAdedi.ToString();
        }
    }

    public void SonrakiLeveleGec() { if (gecerliLevel < 4) { gecerliLevel++; LeveliBaslat(); } }

    public void TekrarDene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void YosunaCarpildi(float ceza)
    {
        if (gecerliLevel != 4)
        {
            kalanSure -= ceza;
            if (kalanSure < 0) kalanSure = 0;
            SureyiGuncelle();
        }
    }

    void ArayuzuGuncelle()
    {
        if (skorYazisi != null) skorYazisi.text = suAnkiPuan + " TL";
        if (adetYazisi != null) adetYazisi.text = (gecerliLevel == 4) ? suAnkiBalikAdedi.ToString() : suAnkiBalikAdedi + " / " + hedefBalikSayisi;
    }

    void SureyiGuncelle()
    {
        float gs = Mathf.Max(0, kalanSure);
        int dk = Mathf.FloorToInt(gs / 60);
        int sn = Mathf.FloorToInt(gs % 60);
        if (sureYazisi != null) sureYazisi.text = string.Format("{0:00}:{1:00}", dk, sn);
    }

    // YENİ: Menüye dönerken oyunu duraklatmadan çıkmasını garantiliyoruz
    public void AnaMenuyeDon()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Giris");
    }

    public void OyunuKapat() { Application.Quit(); }
}