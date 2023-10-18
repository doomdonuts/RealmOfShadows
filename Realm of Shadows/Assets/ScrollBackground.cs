using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float speed = 0.1f;
    public float currentScroll;
    Material material;
    
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        currentScroll += speed * Time.deltaTime;
        material.mainTextureOffset = new Vector2(currentScroll, 0);
    }
}
