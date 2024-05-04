using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


    public class OptionSlider : MonoBehaviour
    {
        [Header("UI")]
        public Slider slider;
        public TMP_InputField inputField;

        private AudioSource changeSound;

        public void Start()
        {
            changeSound = GetComponent<AudioSource>();
        }

        public void SliderChange()
        {
            if (gameObject.name == "SensSlider")
            {
                var val = (decimal) slider.value / 100;
                var valString = val.ToString(CultureInfo.InvariantCulture);
                inputField.text = valString;
            }
            else
            {
                inputField.text = slider.value.ToString(CultureInfo.InvariantCulture);
            }

            // Audio
            if(changeSound == null)
            {
                changeSound = GetComponent<AudioSource>();
            }

            //Play sound 
            //if (changeSound.enabled) {
            //    changeSound.pitch = Rumba.Map(slider.value, slider.minValue, slider.maxValue, 0.5f, 1);
            //    changeSound.Play();
            //}
        }

        public void NumberChange()
        {
            if (inputField.text == string.Empty) {
                SliderChange();
            }

            if (gameObject.name == "SensSlider")
            {
                string valString = inputField.text.Replace(',', '.');
                float val = float.Parse(valString, CultureInfo.InvariantCulture);
                slider.value = Mathf.Clamp(val*100, slider.minValue, slider.maxValue);
            }
            else {
                slider.value = Mathf.Clamp(int.Parse(inputField.text), slider.minValue, slider.maxValue);
            }
        }
    }

