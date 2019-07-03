/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-20 13:33:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using XLua;

namespace EZhex1991.EZUnity.Example
{
    public static class GenListForUnityEngine
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(UnityEngine.Application),
            typeof(UnityEngine.AudioClip),
            typeof(UnityEngine.AudioSource),
            typeof(UnityEngine.Animator),
            typeof(UnityEngine.Behaviour),
            typeof(UnityEngine.BoxCollider),
            //typeof(UnityEngine.BoxCollider2D),
            typeof(UnityEngine.Camera),
            typeof(UnityEngine.Collider),
            //typeof(UnityEngine.Collider2D),
            typeof(UnityEngine.Collision),
            //typeof(UnityEngine.Collision2D),
            typeof(UnityEngine.Color),
            //typeof(UnityEngine.Color32),
            typeof(UnityEngine.Component),
            typeof(UnityEngine.Debug),
            typeof(UnityEngine.Font),
            typeof(UnityEngine.FontStyle),
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.GUI),
            typeof(UnityEngine.GUIContent),
            typeof(UnityEngine.GUIStyle),
            //typeof(UnityEngine.Handheld),
            typeof(UnityEngine.Input),
            typeof(UnityEngine.LayerMask),
            typeof(UnityEngine.Light),
            typeof(UnityEngine.Material),
            typeof(UnityEngine.Mathf),
            typeof(UnityEngine.Mesh),
            typeof(UnityEngine.MeshCollider),
            typeof(UnityEngine.MonoBehaviour),
            typeof(UnityEngine.Object),
            typeof(UnityEngine.ParticleSystem),
            typeof(UnityEngine.PhysicMaterial),
            typeof(UnityEngine.Physics),
            typeof(UnityEngine.Physics2D),
            typeof(UnityEngine.PlayerPrefs),
            typeof(UnityEngine.Quaternion),
            typeof(UnityEngine.Random),
            typeof(UnityEngine.Ray),
            typeof(UnityEngine.RaycastHit),
            typeof(UnityEngine.Rect),
            //typeof(UnityEngine.RectOffset),
            typeof(UnityEngine.RectTransform),
            typeof(UnityEngine.RectTransform.Axis),
            typeof(UnityEngine.RectTransform.Edge),
            typeof(UnityEngine.Renderer),
            //typeof(UnityEngine.Resources),
            typeof(UnityEngine.Rigidbody),
            //typeof(UnityEngine.Rigidbody2D),
            typeof(UnityEngine.Space),
            typeof(UnityEngine.Screen),
            typeof(UnityEngine.Shader),
            typeof(UnityEngine.SortingLayer),
            typeof(UnityEngine.SphereCollider),
            //typeof(UnityEngine.SpringJoint),
            typeof(UnityEngine.Sprite),
            typeof(UnityEngine.SpriteRenderer),
            typeof(UnityEngine.SystemInfo),
            typeof(UnityEngine.TextAnchor),
            typeof(UnityEngine.TextAsset),
            typeof(UnityEngine.TextMesh),
            typeof(UnityEngine.Texture),
            typeof(UnityEngine.Texture2D),
            //typeof(UnityEngine.Texture3D),
            typeof(UnityEngine.Time),
            typeof(UnityEngine.Transform),
            typeof(UnityEngine.Vector2),
            typeof(UnityEngine.Vector3),
            typeof(UnityEngine.Vector4),
            typeof(UnityEngine.WaitForEndOfFrame),
            typeof(UnityEngine.WaitForFixedUpdate),
            typeof(UnityEngine.WaitForSeconds),
            //typeof(UnityEngine.WWW),
            //typeof(UnityEngine.WWWForm),
            typeof(UnityEngine.ForceMode),

            typeof(UnityEngine.Events.UnityEvent),

            //typeof(UnityEngine.EventSystems.UIBehaviour),

            typeof(UnityEngine.SceneManagement.Scene),
            typeof(UnityEngine.SceneManagement.SceneManager),
            typeof(UnityEngine.SceneManagement.LoadSceneMode),

            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Button.ButtonClickedEvent),
            typeof(UnityEngine.UI.Graphic),
            //typeof(UnityEngine.UI.GridLayoutGroup),
            //typeof(UnityEngine.UI.GridLayoutGroup.Axis),
            //typeof(UnityEngine.UI.GridLayoutGroup.Constraint),
            //typeof(UnityEngine.UI.GridLayoutGroup.Corner),
            //typeof(UnityEngine.UI.HorizontalLayoutGroup),
            typeof(UnityEngine.UI.Image),
            typeof(UnityEngine.UI.Image.FillMethod),
            typeof(UnityEngine.UI.Image.Origin180),
            typeof(UnityEngine.UI.Image.Origin360),
            typeof(UnityEngine.UI.Image.Origin90),
            typeof(UnityEngine.UI.Image.OriginHorizontal),
            typeof(UnityEngine.UI.Image.OriginVertical),
            typeof(UnityEngine.UI.Image.Type),
            typeof(UnityEngine.UI.InputField),
            typeof(UnityEngine.UI.InputField.OnChangeEvent),
            typeof(UnityEngine.UI.InputField.SubmitEvent),
            //typeof(UnityEngine.UI.LayoutGroup),
            //typeof(UnityEngine.UI.Mask),
            //typeof(UnityEngine.UI.MaskableGraphic),
            //typeof(UnityEngine.UI.MaskableGraphic.CullStateChangedEvent),
            typeof(UnityEngine.UI.Outline),
            typeof(UnityEngine.UI.ScrollRect),
            typeof(UnityEngine.UI.Selectable),
            //typeof(UnityEngine.UI.Selectable.Transition),
            typeof(UnityEngine.UI.Shadow),
            typeof(UnityEngine.UI.Slider),
            typeof(UnityEngine.UI.Slider.SliderEvent),
            typeof(UnityEngine.UI.Text),
            typeof(UnityEngine.UI.Toggle),
            typeof(UnityEngine.UI.Toggle.ToggleEvent),
            typeof(UnityEngine.UI.ToggleGroup),
            //typeof(UnityEngine.UI.VerticalLayoutGroup),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(UnityEngine.Events.UnityAction),
            typeof(UnityEngine.Events.UnityAction<bool>),
            typeof(UnityEngine.Events.UnityAction<float>),
            typeof(UnityEngine.Events.UnityAction<string>),
            typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>() { "UnityEngine.GameObject", "networkView" },
            new List<string>() { "UnityEngine.Handheld", "GetActivityIndicatorStyle" },
            new List<string>() { "UnityEngine.Handheld", "SetActivityIndicatorStyle", "UnityEngine.iOS.ActivityIndicatorStyle" },
            new List<string>() { "UnityEngine.Handheld", "SetActivityIndicatorStyle", "UnityEngine.AndroidActivityIndicatorStyle" },
            new List<string>() { "UnityEngine.Input", "IsJoystickPreconfigured", "System.String" },
            new List<string>() { "UnityEngine.Light", "areaSize" },
            new List<string>() { "UnityEngine.Light", "lightmapBakeType" },
            new List<string>() { "UnityEngine.Light", "SetLightDirty" },
            new List<string>() { "UnityEngine.Light", "shadowRadius" },
            new List<string>() { "UnityEngine.Light", "shadowAngle" },
            new List<string>() { "UnityEngine.MonoBehaviour", "runInEditMode" },
            new List<string>() { "UnityEngine.Physics", "OverlapSphereNonAlloc", "UnityEngine.Vector3", "System.Single", "UnityEngine.Collider[]"},
            new List<string>() { "UnityEngine.Physics2D", "OverlapCollider", "UnityEngine.Collider2D", "UnityEngine.ContactFilter2D", "UnityEngine.Collider2D[]" },
            new List<string>() { "UnityEngine.Texture", "imageContentsHash" },
            new List<string>() { "UnityEngine.Texture2D", "alphaIsTransparency" },
            new List<string>() { "UnityEngine.WWW", "movie" },

            new List<string>() { "UnityEngine.UI.Graphic", "OnRebuildRequested" },
            new List<string>() { "UnityEngine.UI.Text", "OnRebuildRequested" },
        };
    }
}
