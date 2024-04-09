using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [Header ("FlashFX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originalMat;

    private void Start()
    {
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            originalMat = spriteRenderer.material;
        }
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = originalMat;

    }

    private void RedColorBlink()
    {
        if(spriteRenderer.color != Color.white) 
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelBlink()
    {
        CancelInvoke();
    }

}
