/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-11 13:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class Menu : MonoBehaviour
    {
        public Button button_Home;
        public Button button_LuaBehaviour;
        public Button button_LuckyBall;
        public Button button_SpaceShooter;

        public GameObject panel_Menu;

        public void Awake()
        {
            if (EZApplication.Instance.runMode == RunMode.Update && !EZResources.Instance.isUpdated)
            {
                panel_Menu.SetActive(false);
                EZResources.Instance.onUpdateCompleteEvent += () =>
                {
                    panel_Menu.SetActive(true);
                };
            }
            button_Home.onClick.AddListener(LoadMenu);
            button_LuaBehaviour.onClick.AddListener(LoadLuaBehaviour);
            button_LuckyBall.onClick.AddListener(LoadLuckyBall);
            button_SpaceShooter.onClick.AddListener(LoadSpaceShooter);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        public void LoadLuaBehaviour()
        {
            SceneManager.LoadScene("LuaBehaviour", LoadSceneMode.Additive);
            panel_Menu.SetActive(false);
        }
        public void LoadLuckyBall()
        {
            SceneManager.LoadScene("LuckyBall", LoadSceneMode.Additive);
            panel_Menu.SetActive(false);
        }
        public void LoadSpaceShooter()
        {
            EZResources.Instance.LoadSceneAsync("spaceshooter", "SpaceShooter", LoadSceneMode.Additive);
            panel_Menu.SetActive(false);
        }
    }
}
