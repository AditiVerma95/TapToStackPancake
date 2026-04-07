using UnityEngine;
using System;

public class PancakeMover : MonoBehaviour
{
    public static event Action OnPancakePlaced;

    public float moveSpeed = 3f;
    public float moveRange = 3f;

    private bool isDropped = false;
    private bool hasTriggered = false;

    private Rigidbody rb;
    private float startY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        ResetPancake();
    }

    public void ResetPancake()
    {
        isDropped = false;
        hasTriggered = false;

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;

        startY = transform.position.y;
    }

    void Update()
    {
        if (isDropped) return;

        float x = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;
        transform.position = new Vector3(x, startY, transform.position.z);
    }

    public void Drop()
    {
        if (isDropped) return;

        isDropped = true;

        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDropped) return;
        if (hasTriggered) return;

        if (collision.gameObject.CompareTag("Pancake") || collision.gameObject.CompareTag("Ground"))
        {
            hasTriggered = true;

            OnPancakePlaced?.Invoke();

            enabled = false;
        }
    }
}