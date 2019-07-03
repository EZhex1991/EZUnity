/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 13:27:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZButtonPropertyAttribute : PropertyAttribute
    {
        public enum ButtonLayout
        {
            Above,
            Replace,
            Below
        }
        public readonly string buttonLabel;
        public readonly string methodName;
        public ButtonLayout layout;
        public EZButtonPropertyAttribute(string methodName, ButtonLayout layout = ButtonLayout.Above)
        {
            this.buttonLabel = methodName;
            this.methodName = methodName.Replace(" ", "");
            this.layout = layout;
        }
        public EZButtonPropertyAttribute(string buttonLabel, string methodName, ButtonLayout layout = ButtonLayout.Above)
        {
            this.buttonLabel = buttonLabel;
            this.methodName = methodName;
            this.layout = layout;
        }
    }
}
