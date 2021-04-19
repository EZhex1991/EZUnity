/* Author:          ezhex1991@outlook.com
 * CreateTime:      2021-01-11 14:13:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class EZFileUtil
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenFileName
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;
            public string filter = null;
            public string customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public string file = null;
            public int maxFile = 0;
            public string fileTitle = null;
            public int maxFileTitle = 0;
            public string initialDir = null;
            public string title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public string defExt = null;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public string templateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;

            public OpenFileName(string title, string filter)
            {
                structSize = Marshal.SizeOf(this);
                this.filter = filter;
                file = new string(new char[256]);
                maxFile = file.Length;
                fileTitle = new string(new char[64]);
                maxFileTitle = fileTitle.Length;
                initialDir = Application.persistentDataPath.Replace("/", "\\");
                this.title = title;
                flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            }

            public bool OpenFile()
            {
                return GetOpenFileName(this);
            }
            public bool SaveFile()
            {
                return GetSaveFileName(this);
            }
        }

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName openFileName);
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName openFileName);
    }
}
#endif
