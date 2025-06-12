using UnityEngine;
using UnityEngine.InputSystem;

public class WolfEyesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wolfEyeRightPrefab;
    [SerializeField] private GameObject wolfEyeLeftPrefab;
    [SerializeField] private float delta = 0f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private int count = 0;
    [SerializeField] public GameManager gameManager;

    void Update()
    {
        if (gameManager.IsGameOver())
        {
            Debug.Log("IsGameOver‚ªŒÄ‚Î‚ê‚ÄŠÒ‚³‚ê‚Ü‚µ‚½");
            return;
        }
        else
        {
            Create();
        }

    }

    public void Create()
    {
        this.delta += Time.deltaTime;
        this.timer += Time.deltaTime;
        if (this.timer < 30f)
        {

            if (this.delta > 3 && count == 0)
            {
                count += 1;
                GameObject go = Instantiate(wolfEyeRightPrefab);
                int px = Random.Range(-7, 0);
                go.transform.position = new Vector3(px, 2, 0);
            }
            else if (this.delta > 8 && count == 1)
            {
                count += 1;
                GameObject go = Instantiate(wolfEyeLeftPrefab);
                int px = Random.Range(0, 8);
                go.transform.position = new Vector3(px, 2, 0);
            }
            else if (this.delta > 12 && count == 2)
            {
                count += 1;
                GameObject go = Instantiate(wolfEyeRightPrefab);
                int px = Random.Range(-7, 0);
                go.transform.position = new Vector3(px, 2, 0);
            }
            else if (this.delta > 16 && count == 3)
            {
                count += 1;
                GameObject go = Instantiate(wolfEyeLeftPrefab);
                int px = Random.Range(0, 8);
                go.transform.position = new Vector3(px, 2, 0);
            }
            else if (this.delta > 20 && count == 4)
            {
                count += 1;
                GameObject go = Instantiate(wolfEyeRightPrefab);
                int px = Random.Range(-7, 0);
                go.transform.position = new Vector3(px, 2, 0);
            }
            else if (this.delta > 25 && count == 5)
            {
                count += 1;
                GameObject go1 = Instantiate(wolfEyeRightPrefab);
                GameObject go2 = Instantiate(wolfEyeLeftPrefab);
                go1.transform.position = new Vector3(-5, 2, 0);
                go2.transform.position = new Vector3(5, 2, 0);
            }
        }

    }
}
