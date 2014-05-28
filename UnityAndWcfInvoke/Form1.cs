using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Common.ServiceProxyFactory.WCF;
using Interface;
using ServiceInterface;

namespace UnityAndWcfInvoke
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ServiceLoader.LoadService<ILogger>().Write());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ServiceLoader.LoadService<ILogger>("databaseLoggerr").Write());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //int i = 10;
            //while (i>0)
            //{
            try
            {
                var data = WcfServiceInvoker.Invoke<IOperation, string>(client => client.Write(), "myTestService");
                MessageBox.Show(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
               
            //    i--;
            //}
        }
    }
}
