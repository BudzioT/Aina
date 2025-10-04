using UnityEngine;

public class MovingShit : MonoBehaviour
{
    public float speed = 3f; // units per second

    private float leftBound;
    private float rightBound;
    private Vector3 direction = Vector3.right;

    void Start()
    {
        // Get screen boundaries in world space
        Camera cam = Camera.main;
        float distance = Mathf.Abs(cam.transform.position.z - transform.position.z);

        Vector3 leftEdge = cam.ScreenToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightEdge = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, distance));

        leftBound = leftEdge.x;
        rightBound = rightEdge.x;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // if object moves outside bounds, flip direction
        if (transform.position.x >= rightBound)
        {
            direction = Vector3.left;
        }
        else if (transform.position.x <= leftBound)
        {
            direction = Vector3.right;
        }
    }
}
