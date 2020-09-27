using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundColorChanger : MonoBehaviour {

    [SerializeField] private float maxColor = 226;
    [SerializeField] private float minColor = 195;
    [SerializeField] private float changeAmount = 2f;
    [SerializeField] private float timerMax = .2f;


    private SpriteRenderer spriteRenderer;

    private float timer;

    private float r;
    private float g;
    private float b;

    private enum ColorSpin {
        nr,
        r,
        ng,
        g,
        nb,
        b
    }
    private ColorSpin currentColor;

    private void Awake() {
        timer = timerMax;

        r = maxColor;
        g = maxColor;
        b = minColor;

        currentColor = ColorSpin.nr;
    }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(r / 255, g / 255, b / 255);
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0f) {
            timer += timerMax;

            switch (currentColor) {
                case ColorSpin.nr:
                    r -= changeAmount;
                    if (r <= minColor) {
                        currentColor = ColorSpin.b;
                    }
                    break;
                case ColorSpin.b:
                    b += changeAmount;
                    if (b >= maxColor) {
                        currentColor = ColorSpin.ng;
                    }
                    break;
                case ColorSpin.ng:
                    g -= changeAmount;
                    if (g <= minColor) {
                        currentColor = ColorSpin.r;
                    }
                    break;
                case ColorSpin.r:
                    r += changeAmount;
                    if (r >= maxColor) {
                        currentColor = ColorSpin.nb;
                    }
                    break;
                case ColorSpin.nb:
                    b -= changeAmount;
                    if (b <= minColor) {
                        currentColor = ColorSpin.g;
                    }
                    break;
                case ColorSpin.g:
                    g += changeAmount;
                    if (g >= maxColor) {
                        currentColor = ColorSpin.nr;
                    }
                    break;
            }

            spriteRenderer.color = new Color(r / 255, g / 255, b / 255);
        }
    }
}
