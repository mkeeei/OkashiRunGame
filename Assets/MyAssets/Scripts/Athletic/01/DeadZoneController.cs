using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加

public class DeadZoneController : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float distance = 2.5f; // プレイヤーからの距離
    public float speed = 3.0f; // 追従キャラクターの移動速度

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sheep"))
        {
            // 現在のシーンを再読み込み
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (other.CompareTag("Wolf"))
        {
            // ウルフが画面外に出た場合は、再度画面内に配置
            Vector2 targetPosition = (Vector2)player.position - (Vector2)player.right * distance;

            // 追従キャラクターをターゲット位置に向けて移動
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // プレイヤーを常に注視
            Vector2 direction = (Vector2)player.position - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
