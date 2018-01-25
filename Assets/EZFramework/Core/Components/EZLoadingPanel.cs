/*
 * Author:      熊哲
 * CreateTime:  11/1/2016 5:21:11 PM
 * Description:
 * 
*/
using UnityEngine;
using UnityEngine.UI;

namespace EZFramework
{
    public class EZLoadingPanel : MonoBehaviour
    {
        public RawImage background;
        public RawImage mainImage;
        public Text title;

        public Slider slider_Progress;
        public Text text_Percentage;
        public Text text_Progress;

        void Awake()
        {
            gameObject.SetActive(true);
            ShowProgress("", 0);
        }

        public void ShowProgress(string str)
        {
            gameObject.SetActive(true);
            text_Progress.text = str;
        }
        public void ShowProgress(float percentage)
        {
            gameObject.SetActive(true);
            percentage = Mathf.Clamp01(percentage);
            slider_Progress.value = percentage;
            text_Percentage.text = Mathf.RoundToInt(percentage * 100) + "%";
        }
        public void ShowProgress(string str, float percentage)
        {
            gameObject.SetActive(true);
            percentage = Mathf.Clamp01(percentage);
            text_Progress.text = str;
            slider_Progress.value = percentage;
            text_Percentage.text = Mathf.RoundToInt(percentage * 100) + "%";
        }

        public void LoadComplete()
        {
            ShowProgress("", 0);
            gameObject.SetActive(false);
        }
    }
}