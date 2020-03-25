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
    [TrackClipType(typeof(EZMaterialColorClip))]
    public class EZMaterialColorTrack : TrackAsset
    {
        protected virtual string defaultPropertyName { get { return "_Color"; } }
        protected virtual Color defaultValue { get { return Color.white; } }

        [EZLockedFoldout]
        public EZMaterialColorPlayableMixer template = new EZMaterialColorPlayableMixer();

        protected virtual void Reset()
        {
            template.propertyName = defaultPropertyName;
            template.value = defaultValue;
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<EZMaterialColorPlayableMixer>.Create(graph, template, inputCount);
            template = mixer.GetBehaviour();
            return mixer;
        }
    }

    [System.Serializable]
    public class EZMaterialColorPlayableMixer : PlayableBehaviour
    {
        public int materialIndex = 0;
        public string propertyName;
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color value;

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
            if (string.IsNullOrEmpty(propertyName)) return;
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
            Color outputValue = Color.black;
            for (int i = 0; i < inputCount; i++)
            {
                var inputPlayable = (ScriptPlayable<EZMaterialColorPlayableBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                float inputWeight = playable.GetInputWeight(i);
                totalWeight += inputWeight;

                outputValue += inputBehaviour.value * inputWeight;
            }

            renderer.GetPropertyBlock(propertyBlock, materialIndex);
            outputValue = Color.Lerp(value, outputValue, totalWeight);
            propertyBlock.SetColor(propertyName, outputValue);
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
