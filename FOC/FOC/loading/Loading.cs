using FOC.Main;
using System;
using System.Threading;
using System.Windows.Forms;

namespace FOC
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }
        private void loading()
        {
            try
            {
                panel2.Width += 10;
                if (panel2.Width >= 800)
                {
                    timer1.Enabled = false;
                    //进入主界面
                    Mian mian = new Mian();
                    mian.Show();
                    this.Hide();
                }
                if (panel2.Width == 400)
                {
                    //执行检测更新
                }
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loading();
        }
    }
}
