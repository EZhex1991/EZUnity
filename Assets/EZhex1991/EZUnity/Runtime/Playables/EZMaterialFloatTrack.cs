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
    [TrackClipType(typeof(EZMaterialFloatClip))]
    public class EZMaterialFloatTrack : TrackAsset
    {
        protected virtual string defaultPropertyName { get { return "_Float"; } }
        protected virtual float defaultValue { get { return 0; } }

        [EZLockedFoldout]
        public EZMaterialFloatPlayableMixer template = new EZMaterialFloatPlayableMixer();

        protected virtual void Reset()
        {
            template.propertyName = defaultPropertyName;
            template.value = defaultValue;
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<EZMaterialFloatPlayableMixer>.Create(graph, template, inputCount);
            template = mixer.GetBehaviour();
            return mixer;
        }
    }

    [System.Serializable]
    public class EZMaterialFloatPlayableMixer : PlayableBehaviour
    {
        public int materialIndex;
        public string propertyName;
        public float value;

        private MaterialPropertyBlock propertyBlock;
        private Renderer lastRenderer;
        private int lastIndex;
        private string lastPropertyName;

        public override void OnPlayableCreate(Playable playable)
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public override void OnGraphStop(Playable playable)
        {
            if (lastRenderer != null)
            {
                lastRenderer.SetPropertyBlock(null, lastIndex);
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Renderer renderer = playerData as Renderer;
            if (renderer == null) return;

            if (lastRenderer != null &&
                (lastRenderer != renderer || lastIndex != materialIndex || lastPropertyName != propertyName))
            {
                lastRenderer.SetPropertyBlock(null, lastIndex);
            }
            lastRenderer = renderer;
            lastIndex = materialIndex;
            lastPropertyName = propertyName;
            if (propertyName == null) return;

            int inputCount = playable.GetInputCount();

            float totalWeight = 0;
            float outputValue = 0;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialFloatPlayableBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                outputValue += inputBehaviour.value * inputWeight;
            }

            renderer.GetPropertyBlock(propertyBlock, materialIndex);
            outputValue = Mathf.Lerp(value, outputValue, totalWeight);
            propertyBlock.SetFloat(propertyName, outputValue);
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
