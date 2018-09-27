using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenBg : MonoBehaviour {

    //https://kylewbanks.com/blog/create-fullscreen-background-image-in-unity2d-with-spriterenderer

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        scale *= cameraSize.x / spriteSize.x;
        transform.position = Vector2.zero;
        transform.localScale = scale;
    }
}
