using UnityEngine;
using UnityEngine.UI;

public class MiniGame01Director : MonoBehaviour
{
    [SerializeField] private GameObject timeGarge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.timeGarge = GameObject.Find("TimeGarge");
    }

    // Update is called once per frame
    void Update()
    {
        this.timeGarge.GetComponent<Image>().fillAmount -= Time.deltaTime;
    }
}
