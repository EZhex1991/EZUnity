/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:14:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    [TrackBindingType(typeof(Renderer))]
    [TrackClipType(typeof(EZMaterialPropertyClip))]
    public class EZMaterialPropertyTrack : TrackAsset
    {
        [EZLockedFoldout]
        public EZMaterialPropertyPlayableMixer template = new EZMaterialPropertyPlayableMixer();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<EZMaterialPropertyPlayableMixer>.Create(graph, template, inputCount);
            template = mixer.GetBehaviour();
            return mixer;
        }
    }

    [System.Serializable]
    public class EZMaterialPropertyPlayableMixer : PlayableBehaviour
    {
        public int materialIndex;
        public EZMaterialFloatProperty[] floatProperties;
        public EZMaterialColorProperty[] colorProperties;
        public EZMaterialVectorProperty[] vectorProperties;

        public Dictionary<string, float> floatMap = new Dictionary<string, float>();
        public Dictionary<string, Vector4> colorMap = new Dictionary<string, Vector4>();
        public Dictionary<string, Vector4> vectorMap = new Dictionary<string, Vector4>();

        private MaterialPropertyBlock propertyBlock;
        private Renderer lastRenderer;
        private int lastIndex;

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

            if (lastRenderer != null && (lastRenderer != renderer || lastIndex != materialIndex))
            {
                lastRenderer.SetPropertyBlock(null, lastIndex);
            }
            lastRenderer = renderer;
            lastIndex = materialIndex;

            int inputCount = playable.GetInputCount();

            floatMap.Clear();
            colorMap.Clear();
            vectorMap.Clear();

            float totalWeight = 0;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight < 1e-5) continue;
                totalWeight += inputWeight;

                var inputPlayable = (ScriptPlayable<EZMaterialPropertyPlayableBehaviour>)playable.GetInput(i);
                var inputBehaviour = inputPlayable.GetBehaviour();

                for (int j = 0; j < inputBehaviour.floatProperties.Length; j++)
                {
                    var property = inputBehaviour.floatProperties[j];
                    if (string.IsNullOrEmpty(property.propertyName)) continue;
                    if (floatMap.ContainsKey(property.propertyName))
                    {
                        floatMap[property.propertyName] += property.value * inputWeight;
                    }
                    else
                    {
                        floatMap.Add(property.propertyName, property.value * inputWeight);
                    }
                }
                for (int j = 0; j < inputBehaviour.colorProperties.Length; j++)
                {
                    var property = inputBehaviour.colorProperties[j];
                    if (string.IsNullOrEmpty(property.propertyName)) continue;
                    if (colorMap.ContainsKey(property.propertyName))
                    {
                        colorMap[property.propertyName] += (Vector4)(property.value * inputWeight);
                    }
                    else
                    {
                        colorMap.Add(property.propertyName, property.value * inputWeight);
                    }
                }
                for (int j = 0; j < inputBehaviour.vectorProperties.Length; j++)
                {
                    var property = inputBehaviour.vectorProperties[j];
                    if (string.IsNullOrEmpty(property.propertyName)) continue;
                    if (vectorMap.ContainsKey(property.propertyName))
                    {
                        vectorMap[property.propertyName] += property.value * inputWeight;
                    }
                    else
                    {
                        vectorMap.Add(property.propertyName, property.value * inputWeight);
                    }
                }
            }

            for (int i = 0; i < floatProperties.Length; i++)
            {
                var property = floatProperties[i];
                if (string.IsNullOrEmpty(property.propertyName)) continue;
                if (floatMap.ContainsKey(property.propertyName))
                {
                    floatMap[property.propertyName] = Mathf.Lerp(property.value, floatMap[property.propertyName], totalWeight);
                }
                else
                {
                    floatMap.Add(property.propertyName, property.value);
                }
            }
            for (int i = 0; i < colorProperties.Length; i++)
            {
                var property = colorProperties[i];
                if (string.IsNullOrEmpty(property.propertyName)) continue;
                if (colorMap.ContainsKey(property.propertyName))
                {
                    colorMap[property.propertyName] = Color.Lerp(property.value, colorMap[property.propertyName], totalWeight);
                }
                else
                {
                    colorMap.Add(property.propertyName, property.value);
                }
            }
            for (int i = 0; i < vectorProperties.Length; i++)
            {
                var property = vectorProperties[i];
                if (string.IsNullOrEmpty(property.propertyName)) continue;
                if (vectorMap.ContainsKey(property.propertyName))
                {
                    vectorMap[property.propertyName] = Vector4.Lerp(property.value, vectorMap[property.propertyName], totalWeight);
                }
                else
                {
                    vectorMap.Add(property.propertyName, property.value);
                }
            }

            renderer.GetPropertyBlock(propertyBlock, materialIndex);
            foreach (var property in floatMap)
            {
                propertyBlock.SetFloat(property.Key, property.Value);
            }
            foreach (var property in colorMap)
            {
                propertyBlock.SetColor(property.Key, property.Value);
            }
            foreach (var property in vectorMap)
            {
                propertyBlock.SetVector(property.Key, property.Value);
            }
            renderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}