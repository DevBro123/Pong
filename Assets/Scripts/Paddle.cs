using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;

    private Camera cam;

    private GameObject topCol;
    private GameObject leftCol;
    private GameObject rightCol;

    private float leftLimit = -2;
    private float rightLimit = 2;

    private Vector2 lastScreenSize;

    void Start()
    {
        lastScreenSize = GetScreenSize();

        cam = Camera.main;

        topCol = new GameObject("Top Collider");
        topCol.AddComponent<BoxCollider2D>();
        leftCol = new GameObject("Left Collider");
        leftCol.AddComponent<BoxCollider2D>();
        rightCol = new GameObject("Right Collider");
        rightCol.AddComponent<BoxCollider2D>();

        SetCollidersToScreenEdge();

        scoreText.text = score.ToString();
    }

    private void SetCollidersToScreenEdge()
    {
        Vector3 topLeft = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

        float xExtent = -topLeft.x;
        float yExtent = topLeft.y;

        topCol.AddComponent<BoxCollider2D>();
        topCol.transform.localScale = new Vector3(xExtent * 2, 1, 1);
        topCol.transform.position = new Vector3(0, yExtent + 0.5f, 0);

        leftCol.AddComponent<BoxCollider2D>();
        leftCol.transform.localScale = new Vector3(1, yExtent * 2, 1);
        leftCol.transform.position = new Vector3(-xExtent - 0.5f, 0, 0);

        rightCol.AddComponent<BoxCollider2D>();
        rightCol.transform.localScale = new Vector3(1, yExtent * 2, 1);
        rightCol.transform.position = new Vector3(xExtent + 0.5f, 0, 0);

        leftLimit = -xExtent + transform.localScale.x / 2;
        rightLimit = xExtent - transform.localScale.x / 2;
    }

    void Update()
    {
        Vector2 currentScreenSize = GetScreenSize();

        if (lastScreenSize != currentScreenSize)
        {
            SetCollidersToScreenEdge();
        }

        lastScreenSize = currentScreenSize;

        Vector3 paddlePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0));
        float xPos = Mathf.Clamp(paddlePos.x, leftLimit, rightLimit);
        transform.position = new Vector3(xPos, -4, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        score++;
        scoreText.text = score.ToString();
        collision.rigidbody.velocity = collision.rigidbody.velocity.normalized * (collision.rigidbody.velocity.magnitude + 0.3f);
    }

    private Vector2 GetScreenSize()
    {
        return new Vector2(Screen.width, Screen.height);
    }
}
