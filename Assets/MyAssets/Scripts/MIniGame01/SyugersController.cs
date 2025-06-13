using UnityEngine;

public class SyugersController : MonoBehaviour
{

    public class SmoothMove : MonoBehaviour
    {
        [SerializeField] public float moveSpeed = 5f; // 移動速度
        [SerializeField] private Vector3 targetPosition; // 目標位置

        void Start()
        {
        }

        void Update()
        {
            // 横方向の入力を取得
            float moveInput = Input.GetAxis("Horizontal");

            // 目標位置を更新
            targetPosition = new Vector3(moveInput * moveSpeed, transform.position.y, transform.position.z);

            // 現在位置から目標位置へ滑らかに移動
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }
    }
}


