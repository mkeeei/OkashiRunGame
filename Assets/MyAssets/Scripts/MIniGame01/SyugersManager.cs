using UnityEngine;

public class SyugersManager : MonoBehaviour
{
    [SerializeField] private GameObject gumiAkaPrefab;
    [SerializeField] private GameObject gumiBluePrefab;
    [SerializeField] private GameObject gumiGreenPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject go1 = Instantiate(gumiGreenPrefab);
        go1.transform.position = new Vector3(-4, 6, 0);
        GameObject go2 = Instantiate(gumiBluePrefab);
        go2.transform.position = new Vector3(-4, 8, 0);
        GameObject go3 = Instantiate(gumiAkaPrefab);
        go3.transform.position = new Vector3(-4, 7, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
