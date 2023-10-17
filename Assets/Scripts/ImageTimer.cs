using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    [SerializeField] private float _maxTime;
    [SerializeField] private UnityEvent _timeOver;

    private Image _progressImage;
    private float _currentTime;
    private bool _isTicked;

    void Start()
    {
        _progressImage = GetComponent<Image>();
        _currentTime = 0;
    }

    void Update()
    {
        if (_isTicked == true)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _isTicked = false;
                _timeOver.Invoke();
            }
        }  
        _progressImage.fillAmount = _currentTime / _maxTime;
    }

    public bool IsTimerRunning()
    {
        return _isTicked; 
    }

    public void Restart()
    {
        _currentTime = _maxTime;
        _isTicked = true;
    }

    public void StopTimer()
    {
        _currentTime = 0;
        _isTicked = false;
    }

}
