using Unity.Cinemachine;
using UnityEngine;

public class MiniGamePlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    private const float JUMP_FORCE = 340.0f;
    private const float WALK_FORCE = 30.0f;
    private const float MAX_WALK_SPEED = 2.0f;
    private bool isSafe = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        this.rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid2D.AddForce(transform.up * JUMP_FORCE);
        }

        if(key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        if (isSafe)
        {
            Debug.Log("ƒZ[ƒt");
        }
        else
        {
            Debug.Log("out");
            //Die();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!isSafe && other.CompareTag("Wolf"))
        {
            Debug.Log("die");
        }
        else if (other.CompareTag("Obstacles"))
        {
            isSafe = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Obstacles"))
        {
            isSafe = false;
        }

    }

}
