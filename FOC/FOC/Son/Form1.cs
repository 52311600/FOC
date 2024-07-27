using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace FOC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getcg();
        }
        /// <summary>
        /// 获取所有串口
        /// </summary>
        private void getcg()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("Available Serial Ports:");
            foreach (string port in ports)
            {
                uiComboBox1.Items.Add(port);
            }
        }
        private SerialPort serialPort;
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string cg = uiComboBox1.Text;
                if (cg != "")
                {
                    serialPort = new SerialPort(cg); // 串口号
                    serialPort.BaudRate = int.Parse(uiComboBox2.Text); // 波特率
                    serialPort.Parity = Parity.None; // 校验位
                    serialPort.DataBits = 8; // 数据位
                    serialPort.StopBits = StopBits.One; // 停止位
                    serialPort.Open();
                }
                uiTextBox1.AppendText(cg + "串口打开成功\r\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
                string cg = uiComboBox1.Text;
                uiTextBox1.AppendText(cg + "串口关闭成功\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "C0";
                data[2] = "00";
                data[3] = uiTextBox2.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "40";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox2.Text = receivedData[1].ToString();
                uiTextBox2.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }
        /// <summary>
        /// 串口发送数据
        /// </summary>
        private void setdata(byte[] sendData)
        {
            // 发送数据
            serialPort.Write(sendData, 0, sendData.Length);
        }
        static byte[] ConvertStringArrayToByteArray(string[] stringArray, int data)
        {

            List<byte> byteArray = new List<byte>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                // 跳过第四个字符串
                if (i == 3)
                {
                    byteArray.Add((byte)data);
                    continue;
                }

                string s = stringArray[i];
                int value;
                if (int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                {
                    byteArray.Add((byte)value);
                }
                else
                {
                    byte byteValue;
                    if (byte.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byteValue))
                    {
                        byteArray.Add(byteValue);
                    }
                    else
                    {
                        // 如果字符串无法解析为整数或字节，则跳过该字符串或者抛出异常
                        // 这里你可以根据需要决定如何处理
                    }
                }
            }
            return byteArray.ToArray();
        }

        static byte[] ConvertStringArrayToByte(string[] stringArray)
        {

            List<byte> byteArray = new List<byte>();
            for (int i = 0; i < stringArray.Length; i++)
            {
                string s = stringArray[i];
                int value;
                if (int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
                {
                    byteArray.Add((byte)value);
                }
                else
                {
                    byte byteValue;
                    if (byte.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byteValue))
                    {
                        byteArray.Add(byteValue);
                    }
                    else
                    {
                        // 如果字符串无法解析为整数或字节，则跳过该字符串或者抛出异常
                        // 这里你可以根据需要决定如何处理
                    }
                }
            }
            return byteArray.ToArray();
        }
        private async void uiSymbolButton6_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = await port.SomeStaticMethod(Convert.ToByte(int.Parse(uiTextBox2.Text)), 1, Convert.ToByte(int.Parse(uiTextBox3.Text)));
                // 发送数据到串口
                serialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void uiSymbolButton16_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = await port.SomeStaticMethod(Convert.ToByte(int.Parse(uiTextBox2.Text)), 9, Convert.ToByte(int.Parse(uiTextBox4.Text)));
                // 发送数据到串口
                serialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton17_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0A";
                data[3] = uiTextBox5.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton18_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0B";
                data[3] = uiTextBox6.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton19_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0C";
                data[3] = uiTextBox7.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton20_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0D";
                data[3] = uiTextBox8.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton21_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0E";
                data[3] = uiTextBox9.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton22_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "0F";
                data[3] = uiTextBox10.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton23_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "10";
                data[3] = uiTextBox11.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton24_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "11";
                data[3] = uiTextBox12.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton25_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "12";
                data[3] = uiTextBox13.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton10_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "00";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton15_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "C1";
                data[2] = "03";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton7_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "10";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton9_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "20";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton11_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "30";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton12_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C1";
                data[2] = "00";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton13_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "C1";
                data[2] = "01";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton14_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = "00";
                data[1] = "C1";
                data[2] = "02";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                setdata(byteArray);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiSymbolButton8_Click(object sender, EventArgs e)
        {
            if (uiComboBox6.Text == "true")
            {
                try
                {
                    string[] data = new string[5];
                    data[0] = uiTextBox2.Text;
                    data[1] = "C0";
                    data[2] = "05";
                    data[3] = "00";
                    data[4] = "AF";

                    // 转换并打印结果
                    byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                    setdata(byteArray);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                try
                {
                    string[] data = new string[5];
                    data[0] = uiTextBox2.Text;
                    data[1] = "C0";
                    data[2] = "06";
                    data[3] = "00";
                    data[4] = "AF";

                    // 转换并打印结果
                    byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                    setdata(byteArray);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void uiSymbolButton37_Click(object sender, EventArgs e)
        {
            try
            {
                int tada = int.Parse(uiTextBox14.Text);
                byte[] data = new byte[5];
                data[0] = (byte)int.Parse(uiTextBox2.Text);
                data[4] = 0xAF;

                if (tada >= 0)
                {
                    data[2] = (byte)(tada / 256);
                    data[3] = (byte)(tada % 256);
                    data[1] = 0xD0;
                }
                else if (tada < 0)
                {
                    data[2] = (byte)(-(tada / 256));
                    data[3] = (byte)(-(tada % 256));
                    data[1] = 0x50;
                }
                setdata(data);
                uiTrackBar2.Value = tada;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "01";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox3.Text = receivedData[1].ToString();
                uiTextBox3.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton26_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "09";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox4.Text = receivedData[1].ToString();
                uiTextBox4.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton27_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0A";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox5.Text = receivedData[1].ToString();
                uiTextBox5.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton28_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0B";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox6.Text = receivedData[1].ToString();
                uiTextBox6.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton29_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0C";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox7.Text = receivedData[1].ToString();
                uiTextBox7.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton30_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0D";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox8.Text = receivedData[1].ToString();
                uiTextBox8.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton31_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0E";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox9.Text = receivedData[1].ToString();
                uiTextBox9.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton32_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "0F";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox10.Text = receivedData[1].ToString();
                uiTextBox10.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton33_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "10";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox11.Text = receivedData[1].ToString();
                uiTextBox11.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton34_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "11";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox12.Text = receivedData[1].ToString();
                uiTextBox12.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton35_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "12";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox13.Text = receivedData[1].ToString();
                uiTextBox13.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton36_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "05";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                int tas = receivedData[2] * 256 + receivedData[1];
                uiTextBox15.Text = tas.ToString();
                uiTextBox15.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton38_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "06";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                int tas = receivedData[2] * 256 + receivedData[1];
                uiTextBox16.Text = tas.ToString();
                uiTextBox16.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }
        private int[] vulue = new int[4];
        private int vulue1;
        private void uiSymbolButton40_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void uiSymbolButton39_Click(object sender, EventArgs e)
        {
            try
            {
                int tada = int.Parse(uiTextBox17.Text);
                byte[] data = new byte[5];
                data[0] = (byte)int.Parse(uiTextBox2.Text);
                data[4] = 0xAF;

                if (tada >= 0)
                {
                    data[2] = (byte)(tada / 256);
                    data[3] = (byte)(tada % 256);
                    data[1] = 0xE0;
                }
                else if (tada < 0)
                {
                    data[2] = (byte)(-(tada / 256));
                    data[3] = (byte)(-(tada % 256));
                    data[1] = 0x60;
                }
                setdata(data);
                uiTrackBar1.Value = tada;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(5);
                int tada = uiTrackBar1.Value;

                byte[] data = new byte[5];
                data[0] = (byte)int.Parse(uiTextBox2.Text);
                data[4] = 0xAF;

                if (tada >= 0)
                {
                    data[2] = (byte)(tada / 256);
                    data[3] = (byte)(tada % 256);
                    data[1] = 0xE0;
                }
                else if (tada < 0)
                {
                    data[2] = (byte)(-(tada / 256));
                    data[3] = (byte)(-(tada % 256));
                    data[1] = 0x60;
                }
                setdata(data);
                uiTextBox17.Text = tada.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uiTextBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTextBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTrackBar2_ValueChanged(object sender, EventArgs e)
        {
            Thread.Sleep(5);
            int tada = uiTrackBar2.Value;

            byte[] data = new byte[5];
            data[0] = (byte)int.Parse(uiTextBox2.Text);
            data[4] = 0xAF;

            if (tada >= 0)
            {
                data[2] = (byte)(tada / 256);
                data[3] = (byte)(tada % 256);
                data[1] = 0xD0;
            }
            else if (tada < 0)
            {
                data[2] = (byte)(-(tada / 256));
                data[3] = (byte)(-(tada % 256));
                data[1] = 0x50;
            }
            setdata(data);
            uiTextBox14.Text = tada.ToString();

        }

        private void uiTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiLabel24_Click(object sender, EventArgs e)
        {

        }

        private void uiTextBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTextBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton42_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "13";
                data[3] = uiTextBox20.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox20.Text = receivedData[1].ToString();
                uiTextBox20.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton41_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "13";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox20.Text = receivedData[1].ToString();
                uiTextBox20.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiTextBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton44_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "14";
                data[3] = uiTextBox21.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox21.Text = receivedData[1].ToString();
                uiTextBox21.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton46_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "15";
                data[3] = uiTextBox22.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox22.Text = receivedData[1].ToString();
                uiTextBox22.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton48_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "C0";
                data[2] = "16";
                data[3] = uiTextBox23.Text;
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox23.Text = receivedData[1].ToString();
                uiTextBox23.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton43_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "14";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox21.Text = receivedData[1].ToString();
                uiTextBox21.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton45_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "15";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox22.Text = receivedData[1].ToString();
                uiTextBox22.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiSymbolButton47_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = new string[5];
                data[0] = uiTextBox2.Text;
                data[1] = "40";
                data[2] = "16";
                data[3] = "00";
                data[4] = "AF";

                // 转换并打印结果
                byte[] byteArray = ConvertStringArrayToByteArray(data, int.Parse(data[3]));
                // 发送数据
                serialPort.Write(byteArray, 0, byteArray.Length);
                Thread.Sleep(100);
                // 接收串口返回的数据
                byte[] receivedData = new byte[serialPort.BytesToRead];
                serialPort.Read(receivedData, 0, receivedData.Length);

                uiTextBox23.Text = receivedData[1].ToString();
                uiTextBox23.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送数据时出错：{ex.Message}");
            }
        }

        private void uiTextBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void uiTrackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            int tada = int.Parse(uiTextBox14.Text);
            byte[] data = new byte[5];
            data[0] = (byte)int.Parse(uiTextBox2.Text);
            data[4] = 0xAF;
            data[2] = 0;
            data[3] = 0;
            data[1] = 0xD0;
            setdata(data);
            uiTrackBar2.Value = 0;
            uiTextBox14.Text = "0"; ;
        }

        private void uiSymbolButton49_Click(object sender, EventArgs e)
        {
            uiSymbolButton5_Click(null, null);
            uiSymbolButton26_Click(null, null);
            uiSymbolButton27_Click(null, null);
            uiSymbolButton28_Click(null, null);
            uiSymbolButton29_Click(null, null);
            uiSymbolButton30_Click(null, null);
            uiSymbolButton31_Click(null, null);
            uiSymbolButton32_Click(null, null);
            uiSymbolButton33_Click(null, null);
            uiSymbolButton34_Click(null, null);
            uiSymbolButton35_Click(null, null);
            uiSymbolButton41_Click(null, null);
            uiSymbolButton43_Click(null, null);
            uiSymbolButton45_Click(null, null);
            uiSymbolButton47_Click(null, null);
        }

        private void uiPanel4_Click(object sender, EventArgs e)
        {

        }
    }
}

