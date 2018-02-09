/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 11:50:51 AM
 * Description:
 * 
 * 关于InitializeOnLoad和InitializeOnLoadMethod，两个标签都可以自定义初始化（一个是类，一个是方法），初始化在编译后会自动执行；
 * 值得注意的是，初始化在打开工程时同样也会执行一次，而每次Unity打开工程后又会编译一次，所以，在开始工程时，方法实际上是执行了两次；
 * 保证只执行一次的方式，就是在初始化时将方法逻辑注册到EditorApplication.update这个delegate上，逻辑执行后移除。
 * 因为该delegate只会在编译完成后执行，所以打开工程但是未进行编译时，初始化注册成功该update不会执行；
 * 而重新编译后，之前的注册就无效了，这时重新注册就会在编译完成后第一帧执行逻辑，执行后移除也就保证了逻辑【只执行一次】；
 * 
 * 对于在初始化中调用了有ScriptableObject类参数，而你没有进行【只执行一次】的处理，那么很有可能第二次传入的参数异常，原因未知；
 */
using UnityEditor;

namespace EZUnityEditor
{
    public class EZKeystoreInitializer
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EZKeystoreObject ezKeystore = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName, false);
            SetKeystore(ezKeystore);
        }

        public static void SetKeystore(EZKeystoreObject ezKeystore)
        {
            if (ezKeystore != null && ezKeystore.keystoreName != "" && ezKeystore.keyAliasName != "")
            {
                PlayerSettings.Android.keystoreName = ezKeystore.keystoreName;
                PlayerSettings.Android.keystorePass = ezKeystore.keystorePass;
                PlayerSettings.Android.keyaliasName = ezKeystore.keyAliasName;
                PlayerSettings.Android.keyaliasPass = ezKeystore.keyAliasPass;
            }
        }
        public static EZKeystoreObject LoadKeystore()
        {
            EZKeystoreObject ezKeystore = EZScriptableObject.Load<EZKeystoreObject>(EZKeystoreObject.AssetName, true);
            ezKeystore.keystoreName = PlayerSettings.Android.keystoreName;
            ezKeystore.keystorePass = PlayerSettings.Android.keystorePass;
            ezKeystore.keyAliasName = PlayerSettings.Android.keyaliasName;
            ezKeystore.keyAliasPass = PlayerSettings.Android.keyaliasPass;
            return ezKeystore;
        }
    }
}