using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    GameObject player;
    public void Start()
    {
        this.player = GameObject.Find("Sheep01_0");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("シーン遷移前");
        if (other.CompareTag("SceneChange")) // colliderタグのオブジェクトと接触した場合
        {
            Debug.Log("シーン遷移");

            SceneManager.LoadScene("MiniGameScene01"); // シーンを切り替える
        }
    }
}

