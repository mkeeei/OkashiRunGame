using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("当たった" + other);
        if (other.CompareTag("Sheep"))         
        {
            SceneManager.LoadScene("MiniGameScene01"); // シーンを切り替える
            Debug.Log("シーン遷移完了");
        }
    }
}