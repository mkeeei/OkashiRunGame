using UnityEngine;

public class MovigStageController : MonoBehaviour
{
    public float speed = 1.0f;  // “®‚«‚Ì‘¬‚³
    public float range = 1.0f;  // ã‰º‚ÌˆÚ“®”ÍˆÍ

    private float startY;

    void Start()
    {
        // ‰ŠúˆÊ’u‚ÌYÀ•W‚ğ‹L˜^
        startY = transform.position.y;
    }

    void Update()
    {
        // Y²•ûŒü‚Éã‰º‚É“®‚©‚·
        float newY = startY + Mathf.Sin(Time.time * speed) * range;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
