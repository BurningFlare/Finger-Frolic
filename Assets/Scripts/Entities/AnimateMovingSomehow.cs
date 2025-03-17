using DG.Tweening;
using System;
using UnityEngine;

public class AnimateMovingSomehow : MonoBehaviour
{
    private Tweener currentTween;
    private Vector2 landingPosition;
    [SerializeField] private float duration = 0.1f;
    public void Move(Rigidbody2D rb, Vector2 direction, Ease easeType = Ease.OutQuad)
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
            rb.position = landingPosition;
        }
        landingPosition = rb.position + direction;
        currentTween = rb.DOMove(landingPosition, duration)
         .SetEase(easeType)
         .OnComplete(() =>
         {
             rb.position = landingPosition;
         });
    }
}
