using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float pancakeHeight = 0.3f;   // height per pancake
    public float followSpeed = 2f;     // LOWER = slower movement

    private float baseY;
    private float targetY;

    void Start()
    {
        baseY = transform.position.y;
        targetY = baseY;
    }

    public void UpdateCameraHeight(int stackCount)
    {
        // start moving after 5 pancakes
        int extra = Mathf.Max(0, stackCount - 5);

        targetY = baseY + extra * pancakeHeight;
    }

    void LateUpdate()
    {
        // smooth follow every frame (NOT instant jump)
        float newY = Mathf.Lerp(
            transform.position.y,
            targetY,
            Time.deltaTime * followSpeed
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}