using UnityEngine;

public class LogoHover : MonoBehaviour
{
    public float speed = 3f;  // “®‚«‚Ì‘¬‚³
    public float range = 0.15f;  // ã‰º‚ÌˆÚ“®”ÍˆÍ

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