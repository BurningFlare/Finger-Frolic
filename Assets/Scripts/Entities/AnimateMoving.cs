using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class AnimateMoving : MonoBehaviour
{
    private Tweener _currentTween;
    private Coroutine _nextFrameUpdater;
    private Vector2 _landingPosition;
    private Vector2 _originalPosition;
    private Rigidbody2D _rb;
    [SerializeField] private float duration = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, Ease easeType = Ease.OutQuad)
    {
        _originalPosition = _rb.position;
        if (IsActive())
        {
            _currentTween.Kill();
            _rb.position = _landingPosition;

            if (_nextFrameUpdater != null)
            {
                StopCoroutine(_nextFrameUpdater);
            }

            IEnumerator NextFrameWaiter()
            {
                yield return new WaitForEndOfFrame();
                TweenToEnd(_rb, direction, easeType);
            }

            _nextFrameUpdater = StartCoroutine(NextFrameWaiter());
        }
        else
        {
            TweenToEnd(_rb, direction, easeType);
        }
    }

    private void TweenToEnd(Rigidbody2D rb, Vector2 direction, Ease easeType = Ease.OutQuad)
    {
        _landingPosition = rb.position + direction;
        _currentTween = rb.DOMove(_landingPosition, duration)
         .SetEase(easeType)
         .OnComplete(() =>
         {
             rb.position = _landingPosition;
         });
    }

    public void RejectMove(Rigidbody2D rb, Ease easeType = Ease.OutQuad)
    {
        if (IsActive())
        {
            float remainingDuration = duration - _currentTween.Elapsed();
            _currentTween.Kill();
            Vector2 temp = _landingPosition;
            _landingPosition = _originalPosition;
            _originalPosition = temp;
            _currentTween = rb.DOMove(_landingPosition, remainingDuration)
             .SetEase(easeType)
             .OnComplete(() =>
             {
                 rb.position = _landingPosition;
             });
        }
    }

    public bool IsActive()
    {
        return _currentTween != null && _currentTween.IsActive();
    }
}
