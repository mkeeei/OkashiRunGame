using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class SyugersController : MonoBehaviour
{
    private Rigidbody2D rigid2D;
    [SerializeField] private int key = 1;
    public float walkForce = 30.0f;
    public float maxWalkSpeed = 5.0f;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.Log(key);
        
        float speedx = Mathf.Abs(rigid2D.linearVelocity.x);
        if (speedx < maxWalkSpeed)
        {
            var force = transform.right * key * walkForce;
            Debug.Log(force);
            //this.rigid2D.linearVelocityX = -this.rigid2D.linearVelocityX;
            this.rigid2D.AddForce(force);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.CompareTag("GumiGreen") || collision.collider.CompareTag("GumiRed") || collision.collider.CompareTag("GumiBlue"))

        {
            Debug.Log("YES");
            key = -(key);
        }
    }

}





