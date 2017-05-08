/*
 * Author:      熊哲
 * CreateTime:  9/30/2016 11:08:25 AM
 * Description:
 * 监测退出按钮，连续按键退出
*/
using UnityEngine;

namespace EZFramework
{
    public class ExitButtonListener : MonoBehaviour
    {
        public GameObject hint;
        public float repeatTime = 1.0f;

        private bool clicked = false;
        private float leftTime = 0;

        void Start()
        {
            hint.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }

            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
            }
            else
            {
                clicked = false;
                hint.SetActive(false);
            }
        }

        void Exit()
        {
            if (clicked)
            {
                Application.Quit();
            }
            else
            {
                clicked = true;
                leftTime = repeatTime;
                hint.SetActive(true);
                hint.transform.SetAsLastSibling();
            }
        }
    }
}