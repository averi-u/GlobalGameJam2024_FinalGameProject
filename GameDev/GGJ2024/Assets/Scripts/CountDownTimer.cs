using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    private float countdownTime = 60f;
    public Slider _Slider;
    public float _currentTime;
    
    void Start()
    {
        _Slider = GetComponent<Slider>();
        _currentTime = 30f;
    }

    void Update()
    {
		_currentTime -= Time.deltaTime;
        _Slider.value = _currentTime / countdownTime;
		
        if (_currentTime > 0)
        {
            
        }
        else
        {
            GSys.instance.EndGame();

		}
    }
    public void Add_5_Seconds()
    {
        _currentTime += 5;
    }
}