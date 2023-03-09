using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pref_Panel_Slider : MonoBehaviour
{
    public Slider slider;
    public InputField inpField;
    public Text placeHolderText;
    public Text text;

    private void Start()
    {
        ChangeTextValue();
        ChangeSliderValue();
    }

    public void ChangeTextValue()
    {
        inpField.text = slider.value.ToString();
    }

    public void ChangeSliderValue()
    {
        placeHolderText.text = text.text;
        slider.value = float.Parse(placeHolderText.text.Replace('.', ','));
        text.text = "";
    }
}
