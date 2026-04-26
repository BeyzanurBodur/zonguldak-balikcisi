using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float speed = 0.5f;

    [Tooltip("Eksi (-) deðerler objeyi arkaya, artý (+) deðerler öne alýr.")]
    public int katmanSirasi = -5;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Ýþte sihirli satýr: Quad'a 2D katman sýrasý veriyoruz!
        rend.sortingOrder = katmanSirasi;
    }

    void Update()
    {
        float offset = Time.time * speed;
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}