using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class GaugeScripts : MonoBehaviour {

	public Slider slider;
	float sliderSpeed = 0.5f;
	void Start () {
        slider.value = 0f;
    }
	
	void Update () {
		ActiveSliderAnimation();
	}

	void ActiveSliderAnimation()
	{
		slider.value += sliderSpeed;
		if(slider.value == slider.maxValue){
			slider.value *= -1;
		}
	}

}
