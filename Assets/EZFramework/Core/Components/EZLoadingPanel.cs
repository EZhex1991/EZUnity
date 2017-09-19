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
            slider_Progress.value = percentage;
            text_Percentage.text = (percentage * 100).ToString("00") + "%";
        }
        public void ShowProgress(string str, float percentage)
        {
            gameObject.SetActive(true);
            text_Progress.text = str;
            slider_Progress.value = percentage;
            text_Percentage.text = (percentage * 100).ToString("00") + "%";
        }

        public void LoadComplete()
        {
            ShowProgress("", 0);
            gameObject.SetActive(false);
        }
    }
}