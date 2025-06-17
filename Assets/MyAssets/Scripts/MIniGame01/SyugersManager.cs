using System.Threading;
using UnityEngine;

public class SyugersManager : MonoBehaviour
{
    [SerializeField] private GameObject gumiAkaPrefab;
    [SerializeField] private GameObject gumiBluePrefab;
    [SerializeField] private GameObject gumiGreenPrefab;
    [SerializeField] private float timer = 0f;
    [SerializeField] private int count = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go1 = Instantiate(gumiGreenPrefab);
        go1.transform.position = new Vector3(-4, -1, 0);
        GameObject go2 = Instantiate(gumiBluePrefab);
        go2.transform.position = new Vector3(-2, -1, 0);
        GameObject go3 = Instantiate(gumiAkaPrefab);
        go3.transform.position = new Vector3(4, -1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 5 && count == 0) 
        {
            count+=1;
            GameObject go2 = Instantiate(gumiGreenPrefab);
            go2.transform.position = new Vector3(-2, 7, 0);
        }
        if (timer >= 15 && count == 1)
        {
            count+= 1;
            GameObject go2 = Instantiate(gumiAkaPrefab);
            go2.transform.position = new Vector3(-2, 6, 0);
            
        }
        if (timer >= 20 && count == 2)
        {
            count += 1;
            GameObject go2 = Instantiate(gumiBluePrefab);
            go2.transform.position = new Vector3(-2, 8, 0);
            
        }
    }
}
