using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestComponent : MonoBehaviour {

    private void Start() {
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        
        Texture2D texture2 = new Texture2D(200, 200);
        //texture2.SetPixelData()
        //texture2.SetPixels(0,0,100,100,colors);
        //texture2.SetPixels32() // 比SetPixels快
        
        var sprite = Sprite.Create(texture2, new Rect(0, 0, 200, 200), new Vector2(0.5f, 0.5f));
        
        spriteRenderer.sprite = sprite;
        
        

    }
}
