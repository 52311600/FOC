using System;
using System.Threading.Tasks;

namespace FOC
{
    public class port
    {
        /// <summary>
        /// 构造写入串口数据
        /// </summary>
        /// <param name="Motornumber">电机号</param>
        /// <param name="Registerbit">寄存器位</param>
        /// <param name="Writedata">数据</param>
        public static Task<byte[]> SomeStaticMethod(byte Motornumber, byte Registerbit, byte Writedata)
        {
            try
            {
                byte[] data = new byte[5];
                data[0] = Motornumber;
                data[1] = 192;
                data[2] = Registerbit;
                data[3] = Writedata;
                data[4] = 175;

                return Task.FromResult(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 构造读取串口数据
        /// </summary>
        /// <param name="Motornumber">电机号</param>
        /// <param name="Registerbit">寄存器位</param>
        public static Task<byte[]> redrtaticMethod(byte Motornumber, byte Registerbit)
        {
            try
            {
                byte[] data = new byte[5];
                data[0] = Motornumber;
                data[1] = 64;
                data[2] = Registerbit;
                data[3] = 0;
                data[4] = 175;

                return Task.FromResult(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
