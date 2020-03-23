/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:51:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    [TrackBindingType(typeof(Renderer))]
    [TrackClipType(typeof(EZMaterialFloatPropertyClip))]
    public class EZMaterialFloatPropertyTrack : TrackAsset
    {
        [EZLockedFoldout]
        public EZMaterialFloatPropertyMixer template;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EZMaterialFloatPropertyMixer>.Create(graph, template, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var binding = director.GetGenericBinding(this) as Renderer;
            if (binding == null) return;
            driver.AddFromComponent(binding.gameObject, binding);
            base.GatherProperties(director, driver);
        }
    }

    [System.Serializable]
    public class EZMaterialFloatPropertyMixer : PlayableBehaviour
    {
        public string propertyName = "_Value";
        public float defaultValue = 0;

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
            float value = 0;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialFloatPropertyBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                value += inputBehaviour.value * inputWeight;
            }

            controller.GetPropertyBlock(propertyBlock);
            value = Mathf.Lerp(defaultValue, value, totalWeight);
            propertyBlock.SetFloat(propertyName, value);
            controller.SetPropertyBlock(propertyBlock);
        }
    }
}
