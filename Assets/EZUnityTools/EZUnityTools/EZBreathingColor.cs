/*
 * Author:      熊哲
 * CreateTime:  8/21/2017 10:56:38 AM
 * Description:
 * 
*/
using UnityEngine;
using UnityEngine.UI;

namespace EZUnityTools
{
    [RequireComponent(typeof(Graphic))]
    public class EZBreathingColor : EZBreathingEffects
    {
        [SerializeField]
        private Color m_Color1 = Color.clear;
        public Color color1 { get { return m_Color1; } set { m_Color1 = value; } }

        [SerializeField]
        private Color m_Color2 = Color.white;
        public Color color2 { get { return m_Color2; } set { m_Color2 = value; } }

        private Graphic graphic;

        void Awake()
        {
            graphic = GetComponent<Graphic>();
        }

        public override void DoEffects(float lerp)
        {
            graphic.color = Color.Lerp(color1, color2, lerp);
        }
    }
}