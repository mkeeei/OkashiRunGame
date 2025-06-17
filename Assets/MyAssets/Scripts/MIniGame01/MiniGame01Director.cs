using UnityEngine;
using UnityEngine.UI;

public class MiniGame01Director : MonoBehaviour
{
    //[SerializeField] private GameObject timeGarge;
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    this.timeGarge = GameObject.Find("TimeGarge");
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    this.timeGarge.GetComponent<Image>().fillAmount -= Time.deltaTime;
    //}

    public Image timeGarge;
    public float duration = 30f; // 30ïbä‘Ç≈å∏è≠Ç≥ÇπÇÈ

    private float elapsedTime = 0f;

    void Update()
    {
        if (timeGarge != null)
        {
            elapsedTime += Time.deltaTime;
            timeGarge.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);
        }
    }
}
