using UnityEngine;

public class WolfChaseController : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float distance = 5.0f; // プレイヤーからの距離
    public float speed = 2.0f; // 追従キャラクターの移動速度

    void Update()
    {
        // プレイヤーの後ろから一定の距離を保つターゲット位置を計算
        Vector2 targetPosition = (Vector2)player.position - (Vector2)player.right * distance;

        // 追従キャラクターをターゲット位置に向けて移動
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // プレイヤーを常に注視
        Vector2 direction = (Vector2)player.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

