using UnityEngine;

public class PanelSesi : MonoBehaviour
{
    public AudioSource panelSesKaynagi;
    public AudioClip acilisMuzigi;

    // OnEnable: Bu panel görünür olduðu AN (Level atlayýnca veya ölünce) otomatik çalýþýr!
    void OnEnable()
    {
        if (panelSesKaynagi != null && acilisMuzigi != null)
        {
            panelSesKaynagi.PlayOneShot(acilisMuzigi);
        }

        // Ýstersen panel açýlýnca arkadaki tekneyi susturan kodu da buraya koyabiliriz:
        if (OzelSes.instance != null)
        {
            // Eðer iskele sesleri veya deniz sesleri varsa susturmak için bir komut eklenebilir.
        }
    }
}