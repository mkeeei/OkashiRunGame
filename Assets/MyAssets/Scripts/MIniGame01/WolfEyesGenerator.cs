using UnityEngine;
using UnityEngine.InputSystem;

public class WolfEyesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject wolfEyesPrefab;
    public float span;
    [SerializeField] private float delta = 0f;
    [SerializeField] private float timer = 0f;
    [SerializeField] private int count = 0;

    void Update()
    {
        this.delta += Time.deltaTime;
        this.timer += Time.deltaTime;
        if (this.timer < 30f)
        {

            if (this.delta > span && count%2 ==0)
            {
                this.delta = 0;
                GameObject go = Instantiate(wolfEyesPrefab);
                int px = Random.Range(-8, 0);
                go.transform.position = new Vector3(px, 3, 0);
            }
            else if(this.delta > span && count % 2 == 1)
            {
                this.delta = 0;
                GameObject go = Instantiate(wolfEyesPrefab);
                transform.localScale = new Vector3(-1, 1, 1);
                int px = Random.Range(0, 9);
                go.transform.position = new Vector3(px, 3, 0);
            }
        }
    }
}
