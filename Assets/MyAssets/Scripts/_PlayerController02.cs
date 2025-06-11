using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;
    private bool isJumping;

    private void Start()
    {
        //Rigidbody2Dを取得
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
        Jump();
    }

    private void Jump()
    {
        // isJumpingがfalseの場合にジャンプ可能
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("スペースきがー押されたよ");
            if (!isJumping)
            {
                Debug.Log("ジャンプ！");
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // ジャンプ後、地面に触れるまではジャンプ不可に
                isJumping = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に触れていたらジャンプ可能に
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("地面に触れているよ");
            isJumping = false;
        }
        else
        {
            Debug.Log("地面に触れていないよ");
        }
    }

    private void Run()
    {
        // 入力をxに代入
        float x = Input.GetAxis("Horizontal");

        //x軸に加わる力を格納
        Vector2 force = new Vector2(x * runSpeed, 0 * Time.deltaTime); 

        //Rigidbody2Dに力を加える
        rb.AddForce(force);
    }
}