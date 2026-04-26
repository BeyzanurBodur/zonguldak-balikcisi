using UnityEngine;
using UnityEngine.SceneManagement;

public class AnaMenuKontrol : MonoBehaviour
{
    public void OyunaBasla()
    {
        // Týrnak içindeki isim, senin balýk tuttuðun sahnenin BÝREBÝR adýyla ayný olmalý!
        SceneManager.LoadScene("SampleScene");
    }

    public void OyundanCik()
    {
        Application.Quit();
    }
}