using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynaCompiler
{
    public class MyExecutor
    {
        string inputAssembly;
        string instanceName;
        string methodName;
        public void Execute()
        {
            Assembly assembly = Assembly.LoadFrom(inputAssembly);
            MethodInfo mi;
            Type t = assembly.GetType(instanceName);
            object mode = assembly.CreateInstance(instanceName);
            
            mi = t.GetMethod(methodName);//, BindingFlags.Static | BindingFlags.Public
            mi.Invoke(mode, new object[] { "Hello world !" }); 
        }
        public MyExecutor(string inputAssembly, string instanceName, string methodName)
        {
            this.inputAssembly = inputAssembly;
            this.instanceName = instanceName;
            this.methodName = methodName;
        }
    }
}
