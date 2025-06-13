using UnityEngine;

public class OkashiMovement : MonoBehaviour
{
    //[Header("ˆÚ“®‘¬“x")]
    //[SerializeField] private float speed = 5.0f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.linearVelocityX -= 3f;
        }
    }

    

}
