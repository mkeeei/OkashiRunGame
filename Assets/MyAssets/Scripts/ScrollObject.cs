using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float startPosition;
    [SerializeField] private float endPosition;

    private void Update()
    {
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        if (transform.position.x <= endPosition) ScrollEnd();
    }

    void ScrollEnd()
    {
        float diff = transform.position.x - endPosition;
        Vector3 restartPosition = transform.position;
        restartPosition.x = startPosition + diff;
        transform.position = restartPosition;

        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }
}
