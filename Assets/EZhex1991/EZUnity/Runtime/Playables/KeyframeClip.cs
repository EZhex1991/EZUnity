/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-17 10:35:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public abstract class KeyframeClip<T> : PlayableAsset
        where T : IKeyframe
    {
        public List<T> keyframes = new List<T>();

        public override double duration
        {
            get
            {
                try
                {
                    return keyframes[keyframes.Count - 1].time;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public abstract override Playable CreatePlayable(PlayableGraph graph, GameObject go);
    }
}