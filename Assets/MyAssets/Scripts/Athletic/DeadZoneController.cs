using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加

public class DeadZoneController : MonoBehaviour
{

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
    }
}
