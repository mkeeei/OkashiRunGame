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
            Debug.Log("GUMI!!!!");
            float speedx = Mathf.Abs(rigid2D.linearVelocity.x);
            if (speedx < maxWalkSpeed)
            {
                rigid2D.linearVelocityX += 3.0f;
            } 
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Sheep"))
            {
                key = -key;
            }
        }
    }


   


