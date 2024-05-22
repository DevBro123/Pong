using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;

    [SerializeField] private AudioClip paddleHitSound;
    [SerializeField] private AudioClip wallHitSound;
    [SerializeField] private AudioClip gameOverSound;

    [SerializeField] private AudioSource audioSource;

    private Camera cam;
    private SpriteRenderer renderer;
    private Rigidbody2D rb;

    private bool hasGameStarted;
    private bool endedGame;

    void Start()
    {
        gameOverText.SetActive(false);
        cam = Camera.main;
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        int degree = Random.Range(15, 76);
        int side = (Random.Range(0, 2) == 1) ? -1 : 1;

        rb.AddForce(new Vector2(Mathf.Sin(degree * (Mathf.PI / 180)) * side, Mathf.Cos(degree * (Mathf.PI / 180))) * 1500);
    }

    private void Update()
    {
        if(!renderer.isVisible)
        {
            if (hasGameStarted == false)
            {
                hasGameStarted = true;
            } else if (endedGame == false)
            {
                endedGame = true;
                StartCoroutine(GameOver());
            }
        }
    }

    private IEnumerator GameOver()
    {
        audioSource.clip = gameOverSound;
        audioSource.Play();

        gameOverText.SetActive(true);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("Game");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Vector3 prevDir = rb.velocity.normalized;

        float angle = Mathf.Atan2(prevDir.x, prevDir.y) * (180 / Mathf.PI);
        angle += Random.Range(-6, 7);

        rb.velocity = (new Vector2(Mathf.Sin(angle * (Mathf.PI / 180)), Mathf.Cos(angle * (Mathf.PI / 180)))) * rb.velocity.magnitude;
        
        if (collision.gameObject.GetComponent<Paddle>())
        {
            audioSource.clip = paddleHitSound;
            audioSource.Play();
        } else
        {
            audioSource.clip = wallHitSound;
            audioSource.Play();
        }
    }
}
