using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Canvas canvas;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float deadZone = 0;
    public float handleRange = 1;

    private Vector2 input = Vector2.zero;

    void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(null, background.position);
        Vector2 radius = background.sizeDelta / 2;
        
        float scaleFactor = canvas != null ? canvas.scaleFactor : 1f;

        input = (eventData.position - position) / (radius * scaleFactor);
        
        if (input.magnitude > 1)
            input = input.normalized;

        handle.anchoredPosition = input * radius * handleRange;
        
        Horizontal = (Mathf.Abs(input.x) > deadZone) ? input.x : 0;
        Vertical = (Mathf.Abs(input.y) > deadZone) ? input.y : 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        Horizontal = 0;
        Vertical = 0;
    }
}
