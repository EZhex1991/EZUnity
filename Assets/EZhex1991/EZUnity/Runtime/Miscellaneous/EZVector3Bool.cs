/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 16:54:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    [System.Serializable]
    public struct EZVector3Bool
    {
        public bool x, y, z;

        public EZVector3Bool(bool value)
        {
            x = y = z = value;
        }
        public EZVector3Bool(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
