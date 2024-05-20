using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIElementPlacement : MonoBehaviour
{
    [SerializeField]
    private Canvas panel;
    [SerializeField]
    private RectTransform parent;
    [SerializeField]
    private Vector2 location;

    [SerializeField]
    private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.parent.position + location;

        //parent.localScale = Vector3.one;

        //text.rectTransform.anchoredPosition = new Vector2(parent.position.x - location.x, parent.position.y - location.y);
    }
}
