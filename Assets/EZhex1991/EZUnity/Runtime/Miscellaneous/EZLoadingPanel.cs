/* Author:          ezhex1991@outlook.com
 * CreateTime:      2016-11-01 17:21:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    public class EZLoadingPanel : MonoBehaviour
    {
        public RawImage background;
        public RawImage mainImage;
        public Text title;

        public Slider slider_Progress;
        public Text text_Percentage;
        public Text text_Progress;

        private void Awake()
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

        public void Close()
        {
            ShowProgress("", 0);
            gameObject.SetActive(false);
        }
    }
}