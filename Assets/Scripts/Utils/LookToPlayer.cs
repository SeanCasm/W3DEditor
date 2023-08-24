using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor;

public class LookToPlayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isVisible;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isVisible)
            FollowCamera();
    }
    void OnBecameVisible()
    {
        isVisible = true;
    }
    void OnBecameInvisible()
    {
        isVisible = false;
    }
    public void FollowCamera()
    {
        spriteRenderer.transform.LookAt(PlayerGlobalReference.instance.Position, Vector3.up);
        spriteRenderer.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
