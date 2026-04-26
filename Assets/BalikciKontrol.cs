using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class BalikciKontrol : MonoBehaviour
{
    public enum Durum { Bekliyor, Firlatiliyor, GeriCekiliyor }
    public Durum guncelDurum = Durum.Bekliyor;

    [Header("--- SES AYARLARI ---")]
    public AudioSource sesKaynagi;
    public AudioSource tekneMotorSesKaynagi;
    public AudioClip yurumeSesi, oltaAtmaSesi, oltaCekmeSesi, yosunSesi, paraSesi, tekneMotorSesi;

    [Header("Baðlantýlar")]
    public Transform oltaUcu;
    public Transform kanca;
    public LineRenderer misina;
    public GameObject paraEfektPrefab;
    private Animator anim;
    private MonoBehaviour tekneKontrolScripti;
    private Rigidbody2D tekneRb;

    [Header("Ayarlar")]
    public float firlatmaHizi = 15f;
    public float cekmeHizi = 10f;
    public float maxMenzil = 8f;
    private bool oltaHazir = false;
    private Vector3 hedefNokta;

    [Header("--- ÝSKELE, YÜRÜME VE ARAYÜZ ---")]
    public bool iskeledeMi = true;
    public float yurumeHizi = 5f;
    public GameObject eYazisi;      // "Binmek için E'ye bas"
    public GameObject oltaYazisi;   // "Oltayý almak için Sað Týk yap"
    public GameObject atisYazisi;   // "Atmak için Sol Týk yap"

    private bool binmeNoktasinda = false;
    public float solSinir = -54.14f;
    public float sagSinir = 10f;
    private float devX = 2.96274f;
    private float devY = 2.63065f;

    void Start()
    {
        anim = GetComponent<Animator>();
        iskeledeMi = true;

        if (tekneMotorSesKaynagi != null) tekneMotorSesKaynagi.playOnAwake = false;

        GameObject tekne = GameObject.Find("Taka");
        if (tekne != null)
        {
            tekneKontrolScripti = (MonoBehaviour)tekne.GetComponent("TakaKontrol");
            tekneRb = tekne.GetComponent<Rigidbody2D>();
            if (tekneKontrolScripti != null) tekneKontrolScripti.enabled = false;
            if (tekneRb != null)
            {
                tekneRb.bodyType = RigidbodyType2D.Kinematic;
                tekneRb.useFullKinematicContacts = true;
            }
        }

        if (misina != null) { misina.positionCount = 2; misina.enabled = false; }

        // Baþlangýçta tüm yazýlarý temizleyelim
        if (eYazisi != null) eYazisi.SetActive(false);
        if (oltaYazisi != null) oltaYazisi.SetActive(false);
        if (atisYazisi != null) atisYazisi.SetActive(false);
    }

    void Update()
    {
        if (iskeledeMi)
        {
            YurumeMekanigiveSesi();
            if (binmeNoktasinda && Input.GetKeyDown(KeyCode.E)) GecisiBaslat();
            return;
        }
        OltaMekanigi();
    }

    void YurumeMekanigiveSesi()
    {
        float yatayInput = Input.GetAxisRaw("Horizontal");
        if (yatayInput != 0)
        {
            float yeniX = transform.position.x + (yatayInput * yurumeHizi * Time.deltaTime);
            yeniX = Mathf.Clamp(yeniX, solSinir, sagSinir);
            transform.position = new Vector3(yeniX, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(yatayInput > 0 ? devX : -devX, devY, 1f);

            if (sesKaynagi != null && yurumeSesi != null && !sesKaynagi.isPlaying)
                sesKaynagi.PlayOneShot(yurumeSesi);
        }
        if (anim != null) anim.SetBool("Yuruyor", yatayInput != 0);
    }

    void GecisiBaslat()
    {
        iskeledeMi = false;

        // --- ADIM 1: E'YÝ KAPAT, SAÐ TIK YAZISINI AÇ ---
        if (eYazisi != null) eYazisi.SetActive(false);
        if (oltaYazisi != null) oltaYazisi.SetActive(true);

        if (anim != null) { anim.SetBool("Yuruyor", false); anim.SetBool("TeknedeMi", true); }
        if (tekneRb != null) { tekneRb.bodyType = RigidbodyType2D.Dynamic; tekneRb.WakeUp(); }
        if (tekneKontrolScripti != null) tekneKontrolScripti.enabled = true;
        if (OzelSes.instance != null) OzelSes.instance.IskeleSesleriniKapat();

        if (tekneMotorSesKaynagi != null && tekneMotorSesi != null)
        {
            tekneMotorSesKaynagi.clip = tekneMotorSesi;
            tekneMotorSesKaynagi.loop = true;
            tekneMotorSesKaynagi.Play();
        }
    }

    void OltaMekanigi()
    {
        // YENÝ: Oltayý ele almak için artýk Fare SAÐ TIK (1) kullanýlýyor
        if (Input.GetMouseButtonDown(1))
        {
            oltaHazir = !oltaHazir;
            if (anim != null) anim.SetBool("oltaVar", oltaHazir);

            // --- ADIM 2: SAÐ TIK YAZISINI KAPAT, SOL TIK'I AÇ/KAPAT ---
            if (oltaHazir)
            {
                if (oltaYazisi != null) oltaYazisi.SetActive(false);
                if (atisYazisi != null) atisYazisi.SetActive(true);
            }
            else
            {
                // Eðer oltayý geri býrakýrsa (tekrar sað týka basarsa) geri dön
                if (atisYazisi != null) atisYazisi.SetActive(false);
                if (oltaYazisi != null) oltaYazisi.SetActive(true);
            }

            if (!oltaHazir) { guncelDurum = Durum.Bekliyor; if (misina != null) misina.enabled = false; }
        }

        bool fareUIUzerinde = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        // Oltayý fýrlatmak hala Fare SOL TIK (0)
        if (!fareUIUzerinde && oltaHazir && Input.GetMouseButtonDown(0) && guncelDurum == Durum.Bekliyor)
        {
            AtisYap();

            // --- ADIM 3: ATIÞ YAPILDIÐI AN TÜM YAZILARI SÖNDÜR ---
            if (atisYazisi != null) atisYazisi.SetActive(false);
            if (oltaYazisi != null) oltaYazisi.SetActive(false);

            if (sesKaynagi != null && oltaAtmaSesi != null) sesKaynagi.PlayOneShot(oltaAtmaSesi);
        }

        switch (guncelDurum)
        {
            case Durum.Firlatiliyor: KancayiIleriGotur(); break;
            case Durum.GeriCekiliyor: KancayiGeriCek(); break;
        }

        if (guncelDurum != Durum.Bekliyor && misina != null)
        {
            misina.enabled = true;
            misina.SetPosition(0, oltaUcu.position);
            misina.SetPosition(1, kanca.position);
        }
        else if (misina != null) { misina.enabled = false; kanca.position = oltaUcu.position; }
    }

    void AtisYap() { if (anim != null) { anim.SetTrigger("Atis"); anim.SetBool("bitti", false); } StopCoroutine("IpiFirlatGecikmeli"); StartCoroutine(IpiFirlatGecikmeli(0.3f)); }
    IEnumerator IpiFirlatGecikmeli(float gecikme) { yield return new WaitForSeconds(gecikme); Vector3 farePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); farePos.z = 0; Vector3 yon = (farePos - oltaUcu.position).normalized; hedefNokta = oltaUcu.position + (yon * maxMenzil); guncelDurum = Durum.Firlatiliyor; }

    void KancayiIleriGotur()
    {
        kanca.position = Vector3.MoveTowards(kanca.position, hedefNokta, firlatmaHizi * Time.deltaTime);
        if (Vector3.Distance(kanca.position, hedefNokta) < 0.1f)
        {
            guncelDurum = Durum.GeriCekiliyor;
            if (sesKaynagi != null && oltaCekmeSesi != null) sesKaynagi.PlayOneShot(oltaCekmeSesi);
        }
    }

    void KancayiGeriCek() { kanca.position = Vector3.MoveTowards(kanca.position, oltaUcu.position, cekmeHizi * Time.deltaTime); if (Vector3.Distance(kanca.position, oltaUcu.position) < 0.4f) { guncelDurum = Durum.Bekliyor; BaligiSepeteAt(); if (anim != null) { anim.SetTrigger("Bitti"); anim.SetBool("bitti", true); anim.ResetTrigger("Atis"); } } }

    public void BalikYakalandi(Transform nesne)
    {
        guncelDurum = Durum.GeriCekiliyor;
        nesne.SetParent(kanca);
        nesne.localPosition = Vector3.zero;
        if (sesKaynagi != null && oltaCekmeSesi != null) sesKaynagi.PlayOneShot(oltaCekmeSesi);
        if (nesne.CompareTag("Yosun") && sesKaynagi != null && yosunSesi != null)
            sesKaynagi.PlayOneShot(yosunSesi);
    }

    void BaligiSepeteAt()
    {
        foreach (Transform child in kanca)
        {
            int para = 0;
            if (child.CompareTag("Fish")) para = 5;
            else if (child.CompareTag("BuyukBalik")) para = 250;

            if (para > 0)
            {
                LevelManager.instance?.BalikTutuldu(para);
                if (sesKaynagi != null && paraSesi != null) sesKaynagi.PlayOneShot(paraSesi);
                if (paraEfektPrefab != null)
                {
                    Vector3 spawnPos = transform.position + new Vector3(1f, 1.5f, 0);
                    GameObject efekt = Instantiate(paraEfektPrefab, spawnPos, Quaternion.identity);
                    TMPro.TextMeshPro textMesh = efekt.GetComponent<TMPro.TextMeshPro>();
                    if (textMesh != null) textMesh.text = "+" + para;
                    Destroy(efekt, 2f);
                }
            }
            Destroy(child.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("BinmeNoktasi")) { binmeNoktasinda = true; if (eYazisi != null && iskeledeMi) eYazisi.SetActive(true); } }
    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("BinmeNoktasi")) { binmeNoktasinda = false; if (eYazisi != null) eYazisi.SetActive(false); } }
}