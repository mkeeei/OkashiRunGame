using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePlayerController : MonoBehaviour
{
    //public MiniGamePlayerController miniGamePlayerController;
    [SerializeField] private Rigidbody2D rigid2D;
    public float JUMP_FORCE = 8.0f;
    public float WALK_FORCE = 30.0f;
    public float MAX_WALK_SPEED = 3.0f;
    [SerializeField] private bool isSafe = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isJumping = false;

    public bool IsDead
    {
        get
        {
            //Debug.Log("isDeadÇ™ä“Ç≥ÇÍÇ‹ÇµÇΩ: " + isDead);
            return this.isDead;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isDead + "Ç™ä“Ç≥ÇÍÇ‹ÇµÇΩ");
        if (!isDead)
        {
            Walk();
            Jump();
        }

        //if (transform.position.x > 9)
        //{
        //   GameObject fadeObject = Instantiate(Prefab_Transition);
           //SceneManager.LoadScene("AthleticScene02");
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles"))
        {
            isSafe = true;
            Debug.Log("Obstacles In");
        }

        if (other.CompareTag("Wolf"))
        {
            if (!isSafe)
            {
                //Debug.Log(isDead+"Ç™WolfÇ…êGÇÍÇ‹ÇµÇΩ");
                this.isDead = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Obstacles"))
        {
            isSafe = false;
            Debug.Log("Obstacles Out");
        }

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

    //private void Jump()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        this.rigid2D.AddForce(transform.up * JUMP_FORCE);
    //    }
    //}

    private void Jump()
    {
        //if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Vector2 force = new Vector2(0, JUMP_FORCE);
            this.rigid2D.AddForce(force, ForceMode2D.Impulse);

            isJumping = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
