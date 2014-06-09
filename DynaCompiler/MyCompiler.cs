using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace DynaCompiler
{
    public class MyCompiler
    {
        private StringCollection ReferencedAssemblies
        { get; set; }

        public MyCompiler(StringCollection referencedAssemblies)
        {
            ReferencedAssemblies = referencedAssemblies;
            //ReferencedAssemblies.Add("mscorlib.dll");
            ReferencedAssemblies.Add("System.dll");
        }
        public MyCompiler()
            : this(new StringCollection())
        { }

        /// <summary>
        /// 代码编译类
        /// </summary>
        /// <param name="codeString"></param>
        /// <param name="outputAssembly"></param>
        public void Compile(string codeString, string outputAssembly)
        {
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.CompilerOptions = "/target:library /optimize";

            compilerParameters.OutputAssembly = outputAssembly;
            //生成调试信息
            compilerParameters.IncludeDebugInformation = false;

            //添加相关引用
            foreach (var item in ReferencedAssemblies)
            {
                compilerParameters.ReferencedAssemblies.Add(item);
            }

            //编译
            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerResults results = provider.CompileAssemblyFromSource(compilerParameters, codeString);

            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder();
                foreach (var item in results.Errors)
                {
                    errors.AppendLine(item.ToString());
                }
                throw new Exception(errors.ToString());
            }

            // 创建程序集
            Assembly asm = results.CompiledAssembly;
        }
    }
}
