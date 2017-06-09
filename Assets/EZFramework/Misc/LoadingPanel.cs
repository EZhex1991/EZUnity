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
    public class LoadingPanel : MonoBehaviour
    {
        public Text text_Progress;
        public Slider slider_Progress;

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
        public void ShowProgress(float progress)
        {
            gameObject.SetActive(true);
            slider_Progress.value = progress;
        }
        public void ShowProgress(string str, float progress)
        {
            gameObject.SetActive(true);
            text_Progress.text = str;
            slider_Progress.value = progress;
        }

        public void LoadComplete()
        {
            ShowProgress("", 0);
            gameObject.SetActive(false);
        }
    }
}