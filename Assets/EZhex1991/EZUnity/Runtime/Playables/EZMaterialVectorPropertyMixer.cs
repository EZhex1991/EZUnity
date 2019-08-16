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
    public class EZMaterialVectorPropertyMixer : PlayableBehaviour
    {
        public string propertyName = "_Vector";
        [EZSingleLineVector4]
        public Vector4 defaultValue = Vector4.zero;

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
            Vector4 value = Vector4.zero;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialVectorPropertyBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                value += inputBehaviour.value * inputWeight;
            }

            controller.GetPropertyBlock(propertyBlock);
            value = Vector4.Lerp(defaultValue, value, totalWeight);
            propertyBlock.SetVector(propertyName, value);
            controller.SetPropertyBlock(propertyBlock);
        }
    }
}
