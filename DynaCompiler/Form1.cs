using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.CSharp;

namespace DynaCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var codeString = new MyCodeBuilder().GenerateCode("MPS.API.DAL.Procedure", "OSSDataAccess",
                "\t\t\treturn \"hello\";"); 
            MessageBox.Show(codeString);
            MyCompiler pc = new MyCompiler();
            pc.Compile(codeString, "x.dll");
        }



        //MPS.API.DAL.Procedure.OSSDataAccess
        

        private void button2_Click(object sender, EventArgs e)
        {
            var path = Application.StartupPath;
            MyExecutor ex = new MyExecutor("x.dll", "MPS.API.DAL.Procedure.OSSDataAccess", "Hello");
            ex.Execute();
        }
    }
}
