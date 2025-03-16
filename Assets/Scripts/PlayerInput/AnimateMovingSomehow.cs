using DG.Tweening;
using System;
using UnityEngine;

public class AnimateMovingSomehow : MonoBehaviour
{
    private Tweener currentTween;
    private Vector2 landingPosition;
    [SerializeField]private float duration;

    
    public void Move(Rigidbody2D rb, Vector2 direction, Ease easeType = Ease.OutQuad)
    {

        
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            rb.position = landingPosition;
        }
        landingPosition = rb.position + direction;


        currentTween = rb.DOMove(rb.position + direction, duration).SetEase(easeType);



    }
}
