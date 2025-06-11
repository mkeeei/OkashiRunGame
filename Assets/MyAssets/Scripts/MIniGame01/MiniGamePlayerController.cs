using Unity.Cinemachine;
using UnityEngine;

public class MiniGamePlayerController : MonoBehaviour
{
    public MiniGamePlayerController miniGamePlayerController;
    Rigidbody2D rigid2D;
    private const float JUMP_FORCE = 340.0f;
    private const float WALK_FORCE = 30.0f;
    private const float MAX_WALK_SPEED = 3.0f;
    private bool isSafe = false;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles"))
        {
            isSafe = true;
        }

        if (other.CompareTag("Wolf"))
        {
            if (!isSafe)
            {
                Dead();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Obstacles"))
        {
            isSafe = false;
        }

    }

    private void Dead()
    {
        Debug.Log("Dead");
        // MiniGamePlayerController スクリプトを無効化
        miniGamePlayerController.enabled = false;
        animator.speed = 0;
    }

    private void Walk()
    {
        int key = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            key = 1;
        }

        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);

        if (speedx < MAX_WALK_SPEED)
        {
            this.rigid2D.AddForce(transform.right * key * WALK_FORCE);
        }

        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid2D.AddForce(transform.up * JUMP_FORCE);
        }
    }

}
