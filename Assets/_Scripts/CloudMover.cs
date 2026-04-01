using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float minSpeed = 150f;
    public float maxSpeed = 250f;

    float speed;
    float startY;
    float offset;

    void Start()
    {
        // random speed per cloud
        speed = Random.Range(minSpeed, maxSpeed);

        startY = transform.position.y;
        offset = Random.Range(0f, 100f);
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // move left
        pos.x -= speed * Time.deltaTime;

        // loop
        if (pos.x < -900f)
            pos.x = 900f;

        // slight floating motion
        pos.y = startY + Mathf.Sin(Time.time + offset) * 20f;

        transform.position = pos;
    }
}