using UnityEngine;

public class BalikciTakip : MonoBehaviour
{
    [Tooltip("Takip edilecek gemi objesi. Hiyerarþiden sürükleyin.")]
    public Transform gemi;

    public Vector3 ofset;
    public float gecikmeZamani = 0.15f;
    private Vector3 anlikHiz = Vector3.zero;

    // YENÝ EKLEDÝÐÝMÝZ KISIM:
    private BalikciKontrol anaKontrol;

    void Start()
    {
        anaKontrol = GetComponent<BalikciKontrol>();
    }

    void Update()
    {
        if (anaKontrol != null && anaKontrol.iskeledeMi)
        {
            return;
        }

        // SmoothDamp yerine direkt pozisyon eþitleme yapýyoruz
        // Bu sayede tekne milim oynasa karakter de anýnda onunla gider
        Vector3 hedefPozisyon = gemi.position + ofset;
        transform.position = hedefPozisyon;
    }
}