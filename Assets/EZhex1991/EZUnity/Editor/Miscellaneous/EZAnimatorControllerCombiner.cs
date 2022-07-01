/* Author:          ezhex1991@outlook.com
 * CreateTime:      2022-07-01 19:50:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAnimatorControllerCombiner : EditorWindow
    {
        public class Combiner : ScriptableSingleton<Combiner>
        {
            public bool combineLayers = true;
            public bool combineParameters = true;
            public AnimatorController output;
            public AnimatorController[] elements;

            public bool Combine()
            {
                if (output != null)
                {
                    if (combineLayers)
                    {
                        foreach (AnimatorController controller in elements)
                        {
                            foreach (AnimatorControllerLayer layer in controller.layers)
                            {
                                output.AddLayer(layer);
                            }
                        }
                    }
                    if (combineParameters)
                    {
                        foreach (AnimatorController controller in elements)
                        {
                            foreach (AnimatorControllerParameter parameter in controller.parameters)
                            {
                                output.AddParameter(parameter);
                            }
                        }
                    }
                    EditorUtility.SetDirty(output);
                    return true;
                }
                return false;
            }
        }
        public static Combiner combiner { get { return Combiner.instance; } }

        public static Editor m_CombinerEditor;
        public static Editor combinerEditor
        {
            get
            {
                if (m_CombinerEditor == null)
                {
                    m_CombinerEditor = Editor.CreateEditor(combiner);
                }
                return m_CombinerEditor;
            }
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            combinerEditor.DrawDefaultInspector();

            if (GUILayout.Button("Combine"))
            {
                if (combiner.Combine())
                {
                    Selection.activeObject = combiner.output;
                }
            }
        }
    }
}
