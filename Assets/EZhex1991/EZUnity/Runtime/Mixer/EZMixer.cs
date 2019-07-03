/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-25 19:50:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [Serializable]
    public class EZMixerBlendable
    {
        [SerializeField, Range(0, 1)]
        private float m_Weight = 1;
        public float weight { get { return m_Weight; } set { m_Weight = Mathf.Clamp01(value); } }

        [SerializeField]
        private float m_BlendFactor = 1;
        public float blendFactor { get { return m_BlendFactor; } set { m_BlendFactor = Mathf.Max(0, value); } }

        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_BlendCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve blendCurve { get { return m_BlendCurve; } set { m_BlendCurve = value; } }

        public float outputWeight { get { return blendFactor * blendCurve.Evaluate(weight); } }
        public bool active { get; set; }
    }

    public abstract class EZMixer : MonoBehaviour
    {
        protected abstract EZMixerBlendable[] blendables { get; }
        public int defaultSource = -1;
        public float delta = 0.05f;

        public virtual float IncreaseWeight(int index, bool deactiveDefault = false)
        {
            if (defaultSource >= 0 && deactiveDefault) blendables[defaultSource].active = false;
            blendables[index].active = true;
            return blendables[index].weight;
        }

        protected void Update()
        {
            float totalWeight = 0;
            for (int i = 0; i < blendables.Length; i++)
            {
                blendables[i].weight += blendables[i].active ? delta : -delta;
                totalWeight += blendables[i].outputWeight;
            }
            Mix(totalWeight);
            for (int i = 0; i < blendables.Length; i++)
            {
                blendables[i].active = i == defaultSource;
            }
        }
        protected abstract void Mix(float totalWeight);
    }
}
