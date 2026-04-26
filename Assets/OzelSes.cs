using UnityEngine;

public class OzelSes : MonoBehaviour
{
    // Bu 'instance' sayesinde diðer kodlardan bu sese "Sus!" diyebileceðiz
    public static OzelSes instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Eðer sahnede zaten varsa yenisini sil
        }

        // Sesleri zorla baþlat (az önce iþe yarayan kýsým)
        AudioSource[] sesler = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource s in sesler)
        {
            if (!s.isPlaying) s.Play();
        }
    }

    // ÝÞTE SUSTURUCU KOMUTU: Bunu tekneye binince çaðýracaðýz
    public void IskeleSesleriniKapat()
    {
        AudioSource[] sesler = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource s in sesler)
        {
            s.Stop(); // Hepsini sustur
        }
        // Ýstersen susturduktan sonra bu objeyi komple yok edebilirsin:
        // Destroy(gameObject); 
    }
}