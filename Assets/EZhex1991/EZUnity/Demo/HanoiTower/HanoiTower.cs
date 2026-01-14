/* Author:          #AUTHORNAME#
 * CreateTime:      #CREATETIME#
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity.Demo
{
    public class HanoiTower : MonoBehaviour
    {
        public int layerCount = 5;
        public float interval = 0.1f;
        public Text text;

        private List<Transform> tower = new List<Transform>();
        private Vector3 startPosition;
        private Vector3 tempPosition;
        private Vector3 desPosition;
        private int moveCount;

        void Start()
        {
            startPosition.x = -layerCount;
            desPosition.x = layerCount;
            for (int i = 0; i < layerCount; i++)
            {
                tower.Add(GameObject.CreatePrimitive(PrimitiveType.Cube).transform);
                tower[i].position = new Vector3(startPosition.x, -i, 0);
                tower[i].localScale = Vector3.one + Vector3.right * i;
            }
            StartCoroutine(Move(tower, startPosition, tempPosition, desPosition));
        }

        IEnumerator Move(List<Transform> tower, Vector3 current, Vector3 idle, Vector3 dest)
        {
            if (tower.Count == 1)
            {
                yield return new WaitForSeconds(interval);
                SetX(tower[0], dest.x);
                text.text = $"Move Count: {++moveCount}";
            }
            else
            {
                List<Transform> newTower = new List<Transform>();
                for (int i = 0; i < tower.Count - 1; i++)
                {
                    newTower.Add(tower[i]);
                }
                yield return Move(newTower, current, dest, idle);
                yield return new WaitForSeconds(interval);
                SetX(tower[tower.Count - 1], dest.x);
                text.text = $"Move Count: {++moveCount}";
                yield return Move(newTower, idle, current, dest);
            }
        }

        void SetX(Transform tf, float x)
        {
            Vector3 position = tf.position;
            position.x = x;
            tf.position = position;
        }
    }
}