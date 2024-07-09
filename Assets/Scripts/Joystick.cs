using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform _background;
    private RectTransform _handle;
    private Vector2 _inputVector;

    private void Start()
    {
        _background = GetComponent<RectTransform>();
        _handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, _background.position);
        Vector2 radius = _background.sizeDelta / 2;
        _inputVector = (eventData.position - position) / (radius * 2);

        _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

        _handle.anchoredPosition = new Vector2(_inputVector.x * radius.x, _inputVector.y * radius.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        return _inputVector.x;
    }

    public float Vertical()
    {
        return _inputVector.y;
    }
}
