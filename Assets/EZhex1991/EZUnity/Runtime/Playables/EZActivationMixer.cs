/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-16 16:22:37
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZActivationMixer : PlayableBehaviour
    {
        public Dictionary<GameObject, bool> objectStatus = new Dictionary<GameObject, bool>();

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            objectStatus.Clear();
            for (int i = 0; i < playable.GetInputCount(); i++)
            {
                ScriptPlayable<EZActivationBehaviour> inputPlayable = (ScriptPlayable<EZActivationBehaviour>)playable.GetInput(i);
                EZActivationBehaviour inputBehaviour = inputPlayable.GetBehaviour();
                if (inputBehaviour.gameObject == null) continue;
                if (objectStatus.ContainsKey(inputBehaviour.gameObject))
                {
                    objectStatus[inputBehaviour.gameObject] |= inputPlayable.GetPlayState() == PlayState.Playing;
                }
                else
                {
                    objectStatus[inputBehaviour.gameObject] = inputPlayable.GetPlayState() == PlayState.Playing;
                }
            }
            foreach (var status in objectStatus)
            {
                status.Key.SetActive(status.Value);
            }
        }
    }
}
