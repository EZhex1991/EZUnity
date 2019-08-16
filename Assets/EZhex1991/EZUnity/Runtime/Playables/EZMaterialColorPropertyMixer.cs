/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:52:07
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [System.Serializable]
    public class EZMaterialColorPropertyMixer : PlayableBehaviour
    {
        public string propertyName = "_Color";
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color defaultValue = Color.white;

        private MaterialPropertyBlock m_PropertyBlock;
        private MaterialPropertyBlock propertyBlock
        {
            get
            {
                if (m_PropertyBlock == null)
                {
                    m_PropertyBlock = new MaterialPropertyBlock();
                }
                return m_PropertyBlock;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Renderer controller = playerData as Renderer;
            if (controller == null) return;

            int inputCount = playable.GetInputCount();
            if (inputCount == 0) return;

            float totalWeight = 0;
            Color value = Color.black;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialColorPropertyBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                value += inputBehaviour.value * inputWeight;
            }

            controller.GetPropertyBlock(propertyBlock);
            value = Color.Lerp(defaultValue, value, totalWeight);
            propertyBlock.SetColor(propertyName, value);
            controller.SetPropertyBlock(propertyBlock);
        }
    }
}
