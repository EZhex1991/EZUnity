/*
 * Author:      熊哲
 * CreateTime:  3/31/2017 6:12:01 PM
 * Description:
 * 
*/
#if XLUA && DOTWEEN
using System;
using System.Collections.Generic;
using XLua;

namespace EZUnity.XLuaExtension
{
    public static class GenListForDOTween
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(DG.Tweening.DOTween),
            typeof(DG.Tweening.Sequence),
            typeof(DG.Tweening.Tween),
            typeof(DG.Tweening.Tweener),
            typeof(DG.Tweening.AutoPlay),
            typeof(DG.Tweening.AxisConstraint),
            typeof(DG.Tweening.Ease),
            typeof(DG.Tweening.LoopType),
            typeof(DG.Tweening.PathMode),
            typeof(DG.Tweening.PathType),
            typeof(DG.Tweening.RotateMode),
            typeof(DG.Tweening.ScrambleMode),
            typeof(DG.Tweening.TweenExtensions),
            typeof(DG.Tweening.TweenSettingsExtensions),
            typeof(DG.Tweening.ShortcutExtensions),
            typeof(DG.Tweening.DOTweenModuleAudio),
            typeof(DG.Tweening.DOTweenModulePhysics),
            typeof(DG.Tweening.DOTweenModulePhysics2D),
            typeof(DG.Tweening.DOTweenModuleSprite),
            typeof(DG.Tweening.DOTweenModuleUI),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(DG.Tweening.TweenCallback),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}
#endif
