/*
 * Author:      熊哲
 * CreateTime:  9/30/2016 11:08:25 AM
 * Description:
 * 监测退出按钮，连续按键退出
*/
using UnityEngine;

namespace EZUnity
{
    public class EZExit : MonoBehaviour
    {
        public GameObject hintObject;
        public float repeatTime = 1.0f;

        private bool clicked = false;
        private float timeLeft = 0;

        public void TryExit()
        {
            if (clicked)
            {
                Application.Quit();
            }
            else
            {
                clicked = true;
                timeLeft = repeatTime;
                if (hintObject != null) hintObject.SetActive(true);
            }
        }

        private void Start()
        {
            if (hintObject != null) hintObject.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TryExit();
            }

            if (timeLeft > 0)
            {
                timeLeft -= Time.unscaledDeltaTime;
            }
            else
            {
                clicked = false;
                if (hintObject != null) hintObject.SetActive(false);
            }
        }
    }
}