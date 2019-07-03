/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-22 10:09:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Text;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class RawByteArray : RawObject
    {
        public byte[] m_Target;
        public object Target { get { return m_Target; } }

        public static byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public RawByteArray(int length)
        {
            m_Target = new byte[length];
        }
        public RawByteArray(byte[] data)
        {
            m_Target = data;
        }

        public byte this[int index]
        {
            get { return m_Target[index]; }
            set { m_Target[index] = value; }
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(m_Target);
        }
    }
}
