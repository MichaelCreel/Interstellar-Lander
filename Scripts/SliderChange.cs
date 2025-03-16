using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderChange : MonoBehaviour, IUpdateVisual
{
    [SerializeField] private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSliderValueChanged(float value)
    {
        FindObjectOfType<Settings>().volume = (int)slider.value;
        FindObjectOfType<Settings>().SaveKeybinds();
    }

    public void UpdateVisual()
    {
        slider.value = FindObjectOfType<Settings>().volume;
    }
}