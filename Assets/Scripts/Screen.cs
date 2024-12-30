using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Screen : MonoBehaviour
{
    [Header("Screen")]
    public RawImage image;
    public int width = 128;
    public int height = 160;
    Texture2D imageTexture;

    [Header("Text")]
    public Camera textCamera;
    public RenderTexture renderTexture;
    public TMP_Text text;
    public RectTransform textPos;

    [Header("Extra")]
    public GameObject invertOverlay;

    [Header("Colors")]
    public Color ST7735_BLACK = new Color(0, 0, 0);
    public Color ST7735_RED = new Color(1, 0, 0);
    public Color ST7735_BLUE = new Color(0, 0, 1);
    public Color ST7735_WHITE = new Color(1, 1, 1);

    // Start is called before the first frame update
    void Awake()
    {
        RenderTexture.active = null;
        
        imageTexture = new Texture2D(width, height);
        imageTexture.filterMode = FilterMode.Point;
        ST7735_FillScreen(ST7735_BLACK);
    }

    public void ST7735_DrawBitmap(int x, int y, Color[] bitmap, int w, int h) {
        imageTexture.SetPixels(x, y, w, h, bitmap);
        Refresh();
    }

    public void ST7735_InvertDisplay(bool b) {
        invertOverlay.SetActive(b);
    }

    public void printf(string s) {
        text.text = s;

        Texture2D textTexture = new Texture2D(width, height);
        textTexture.filterMode = FilterMode.Point;
        textCamera.Render();
        RenderTexture.active = renderTexture;
        textTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        textTexture.Apply(false);
        RenderTexture.active = null;

        Vector2Int bounds = Vector2Int.RoundToInt(text.GetRenderedValues(false));
        Vector2Int start = Vector2Int.RoundToInt(new Vector2(textPos.position.x, (height + textPos.position.y) - bounds.y));

        Color[] pixels = textTexture.GetPixels(start.x, start.y, bounds.x, bounds.y);
        imageTexture.SetPixels(start.x, start.y, bounds.x, bounds.y, pixels);
        Refresh();
    }

    public void ST7735_SetCursor(int x, int y) {
        textPos.position = new Vector2(x * 6, 0 - (y * 10));
    }

    public void ST7735_FillScreen(Color color) {
        ST7735_FillRect(0, 0, width, height, color);
    }

    public void ST7735_FillRect(int x, int y, int w, int h, Color color) {
        Color[] pixels = new Color[w * h];

        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = color;
        }
        imageTexture.SetPixels(x, (height - y) - h, w, h, pixels);
        Refresh();
    }

    void Refresh() {
        imageTexture.Apply(false);
        image.texture = imageTexture;
    }
}
