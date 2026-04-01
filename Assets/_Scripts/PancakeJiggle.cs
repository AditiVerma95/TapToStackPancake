using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PancakeJiggle : MonoBehaviour
{
    [Header("Jiggle Settings")]
    [SerializeField] private float squashAmount = 0.3f;
    [SerializeField] private float stretchAmount = 0.15f;
    [SerializeField] private float duration = 0.08f;
    [SerializeField] private int vibrato = 2;

    private Vector3 originalScale;
    private Rigidbody rb;
    private Tween currentTween;
    private bool hasLanded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        // Good defaults for stability
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        hasLanded = true;

        PlayJiggle();
    }

    void PlayJiggle()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();

        transform.localScale = originalScale;

        Vector3 squash = new Vector3(
            originalScale.x + squashAmount,
            originalScale.y - squashAmount,
            originalScale.z + squashAmount
        );

        Vector3 stretch = new Vector3(
            originalScale.x - stretchAmount,
            originalScale.y + stretchAmount,
            originalScale.z - stretchAmount
        );

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(squash, duration).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(stretch, duration).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(originalScale, duration).SetEase(Ease.OutQuad));

        // subtle extra wobble
        seq.Append(transform.DOScale(originalScale, duration * 1.5f)
            .SetEase(Ease.OutElastic)
            .SetLoops(vibrato, LoopType.Yoyo));

        currentTween = seq;
    }
}