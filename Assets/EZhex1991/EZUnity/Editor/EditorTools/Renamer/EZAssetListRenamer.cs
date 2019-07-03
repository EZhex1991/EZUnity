/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-22 19:09:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZAssetListRenamer", menuName = "EZUnity/EZAssetListRenamer", order = (int)EZAssetMenuOrder.EZAssetListRenamer)]
    public class EZAssetListRenamer : ScriptableObject
    {
        public float indexStep = 1;
        public float indexOffset = 1;
        public string indexFormat = "{0:D2}-";

        public string captureRegex = "([0-9]+)-";

        public UnityEngine.Object[] objectList;

        public void Execute()
        {
            Regex reg = new Regex(captureRegex);
            try
            {
                for (int i = 0; i < objectList.Length; i++)
                {
                    if (objectList[i] == null) continue;
                    string oldName = objectList[i].name;
                    string oldPath = AssetDatabase.GetAssetPath(objectList[i]);
                    int index = (int)(i * indexStep + indexOffset);
                    string newName;
                    if (reg.IsMatch(oldName))
                    {
                        newName = reg.Replace(oldName, (match) => string.Format(indexFormat, index));
                    }
                    else
                    {
                        newName = string.Format(indexFormat, index) + oldName;
                    }
                    AssetDatabase.RenameAsset(oldPath, newName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            AssetDatabase.Refresh();
        }
    }
}
