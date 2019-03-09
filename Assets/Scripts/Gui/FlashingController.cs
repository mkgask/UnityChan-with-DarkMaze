using UnityEngine;
using TMPro;

public class FlashingController : MonoBehaviour
{

    TextMeshProUGUI text;

    private float fade = 1f;
    private float fade_max = 1f;
    private float fade_min = 0f;
    private float fade_scale = -0.01f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        if (fade_max < fade || fade < fade_min) {
            fade_scale *= -1;
        }

        fade += fade_scale;

        Color c = text.color;
        c.a = fade;
        text.color = c;
    }

}