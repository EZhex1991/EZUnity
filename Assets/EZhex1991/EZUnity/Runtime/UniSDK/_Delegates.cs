/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-03-12 13:52:22
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity.UniSDK
{
    public delegate void OnEventCallback1(string info);
    public delegate void OnEventCallback2(string info1, string info2);

    public delegate void OnResultCallback(bool result, string info);

    public delegate void OnDataCallback(int data, string info);
}