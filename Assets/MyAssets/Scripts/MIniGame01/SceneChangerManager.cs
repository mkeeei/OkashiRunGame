using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    public GameObject Prefab_Transition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sheep"))
        {
            //GameObject fadeObject = (GameObject)Instantiate(Prefab_Transition);
            //fadeObject.transform.position = new Vector3(0, 0, 0);
            SceneManager.LoadScene("AthleticScene02");
        }
    }
}
