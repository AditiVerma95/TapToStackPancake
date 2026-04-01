using UnityEngine;
using UnityEngine.InputSystem;

public class PancakeMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float moveRange = 3f;

    private bool isDropped = false;
    private Rigidbody rb;
    private PancakeSpawner spawner;

    private float startY;

    public void Initialize(PancakeSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        startY = transform.position.y;
        
    }

    void Update()
    {
        if (isDropped) return;

        MoveSmooth();



        if (Mouse.current.leftButton.wasPressedThisFrame ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
        {
            Drop();
        }
    }

    void MoveSmooth()
    {
        float x = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2) - moveRange;

        transform.position = new Vector3(x, startY, transform.position.z);
    }

    void Drop()
    {
        isDropped = true;
        rb.useGravity = true;

        // Add slight push down
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDropped) return;

        if (collision.gameObject.CompareTag("Pancake") || collision.gameObject.CompareTag("Ground"))
        {
            isDropped = false; // prevent double trigger

            spawner.SpawnNextPancake();

            enabled = false; // stop this pancake completely
        }
    }
}