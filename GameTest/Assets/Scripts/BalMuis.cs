using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BalMuis : MonoBehaviour, IPointerClickHandler
{
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    [Range(2f, 10f)]
    public float throwForce = 4f;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.name == "Bal")
        {
            touchTimeStart = Time.time;
            startPos = transform.position;
            Debug.Log(startPos.x);

            touchTimeFinish = Time.time + 0.005f;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.mousePosition;
            Debug.Log(endPos.x);
            direction = startPos - endPos;
            GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * throwForce);
        }
    }
}
