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
    [TrackClipType(typeof(EZMaterialVectorClip))]
    public class EZMaterialVectorTrack : TrackAsset
    {
        protected virtual string defaultPropertyName { get { return "_Vector"; } }
        protected virtual Vector4 defaultValue { get { return Vector4.zero; } }

        [EZLockedFoldout]
        public EZMaterialVectorPlayableMixer template = new EZMaterialVectorPlayableMixer();

        protected virtual void Reset()
        {
            template.propertyName = defaultPropertyName;
            template.value = defaultValue;
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<EZMaterialVectorPlayableMixer>.Create(graph, template, inputCount);
            template = mixer.GetBehaviour();
            return mixer;
        }
    }

    [System.Serializable]
    public class EZMaterialVectorPlayableMixer : PlayableBehaviour
    {
        public int materialIndex;
        public string propertyName;
        [EZSingleLineVector4]
        public Vector4 value;

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
            Vector4 outputValue = Vector4.zero;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialVectorPlayableBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                outputValue += inputBehaviour.value * inputWeight;
            }

            renderer.GetPropertyBlock(propertyBlock, materialIndex);
            outputValue = Vector4.Lerp(value, outputValue, totalWeight);
            propertyBlock.SetVector(propertyName, outputValue);
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
