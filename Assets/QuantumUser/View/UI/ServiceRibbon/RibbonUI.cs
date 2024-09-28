using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RibbonUI : MonoBehaviour
{
    private enum AnimationState
    {
        None,
        Open,
        Delay,
        Close,
    }
    private Vector2 _originPoint;

    public event Action<string> OnClose;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _ribbonIcon;
    [SerializeField] private TextMeshProUGUI _ribbonCountText;
    [field: SerializeField] public Vector2 Size { get; private set; }
    [SerializeField] private float _endPoint;

    public AnimationCurve curve;
    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private float _delay = 1.0f;
    
    private float elapsedTime = 0.0f;
    private AnimationState _state = AnimationState.None;
    private int _ribbonCount = 0;

    private string _key;
    void Awake()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        _originPoint = _rectTransform.anchoredPosition;
    }
    void Update()
    {
        switch (_state)
        {
            case AnimationState.None:
                break;
            case AnimationState.Open:
                OpenAnimation(Time.deltaTime);
                break;
            case AnimationState.Delay:
                DelayAnimation(Time.deltaTime);
                break;
            case AnimationState.Close:
                CloseAnimation(Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OpenAnimation(float dt)
    {
        elapsedTime += dt;
        var t = elapsedTime / _duration;
       var curveValue = curve.Evaluate(t);
       var endPoint = (Vector2.right * _endPoint) + _originPoint;
        Vector3 newPosition = Vector3.Lerp(_originPoint, endPoint, curveValue);
        Debug.Log($"{_originPoint} -> {endPoint} = {newPosition}");
        _rectTransform.anchoredPosition = new Vector2(newPosition.x,_rectTransform.anchoredPosition.y);
        
        if (!(elapsedTime >= _duration)) return;
        _state = AnimationState.Delay;
        elapsedTime = 0.0f;
        _rectTransform.anchoredPosition = new Vector2(_endPoint + _originPoint.x,_rectTransform.anchoredPosition.y);
    }
    private void DelayAnimation(float dt)
    {
        elapsedTime += dt;
        if (!(elapsedTime >= _delay)) return;
        _state = AnimationState.Close;
        elapsedTime = 0.0f;
    }
    private void CloseAnimation(float dt)
    {
        elapsedTime += dt;
        var t = elapsedTime / _duration;
        var curveValue = curve.Evaluate(t);
        var endPoint = (Vector2.right * _endPoint) + _originPoint;

        Vector3 newPosition = Vector3.Lerp(endPoint,_originPoint , curveValue);
        _rectTransform.anchoredPosition = new Vector2(newPosition.x,_rectTransform.anchoredPosition.y);

        
        if (!(elapsedTime >= _duration)) return;
        _state = AnimationState.None;
        elapsedTime = 0.0f;
        _ribbonCount = 0;
        _rectTransform.anchoredPosition = new Vector2(_originPoint.x, _rectTransform.anchoredPosition.y);
        OnClose?.Invoke(_key);
    }
    public void ReceiveServiceRibbon(string key, Sprite sprite)
    {
        _key = key;
        if(_ribbonIcon.sprite == null || _ribbonIcon.sprite.name != sprite.name) _ribbonIcon.sprite = sprite;
        
        switch (_state)
        {
            case AnimationState.None:
                _state = AnimationState.Open;
                break;
            case AnimationState.Delay:
                elapsedTime = 0.0f;
                break;
            case AnimationState.Close:
            {
                var progress = elapsedTime  / _duration;
                elapsedTime = (1 - progress) * _duration;
                _state = AnimationState.Open;
                break;
            }
        }
         
        _ribbonCount++;
        if (_ribbonCountText != null)
        {
            _ribbonCountText.SetText($"{_ribbonCount}");
        }

    }
    public void OnDrawGizmos()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        var position = _rectTransform.position;
        var endPoint = (Vector2.right * _endPoint) + (Vector2)position;

        var sizeDelta = Size;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(position, new Vector3(sizeDelta.x, sizeDelta.y, 0.01f));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(endPoint, new Vector3(sizeDelta.x, sizeDelta.y, 0.01f));
    }
    
}
