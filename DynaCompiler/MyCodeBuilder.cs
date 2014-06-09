using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace DynaCompiler
{
    public class MyCodeBuilder
    {

        public CodeCompileUnit CreateExecutionClass(string typeNamespace, string typeName, string scriptBody)
        {
            CodeCompileUnit unit = new CodeCompileUnit();
            CodeNamespace nameSpace = new CodeNamespace(typeNamespace);
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            unit.Namespaces.Add(nameSpace);

            //基类
            CodeTypeDeclaration baseTypeDeclaration = new CodeTypeDeclaration("ProcedureDataAccess");
            nameSpace.Types.Add(baseTypeDeclaration);

            CodeConstructor stringConstructor = new CodeConstructor();
            stringConstructor.Attributes = MemberAttributes.Public;
            stringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "TestStringParameter"));
            baseTypeDeclaration.Members.Add(stringConstructor);



            //派生类
            // Declares a type that derives from BaseType and names it.
            CodeTypeDeclaration DerivedType = new CodeTypeDeclaration(typeName);
            // The DerivedType class inherits from the BaseType class.
            DerivedType.BaseTypes.Add(new CodeTypeReference("ProcedureDataAccess"));
            // Adds the new type to the namespace object's type collection.
            nameSpace.Types.Add(DerivedType);

            // Declare a constructor that takes a string argument and calls the base class constructor with it.
            CodeConstructor baseStringConstructor = new CodeConstructor();
            baseStringConstructor.Attributes = MemberAttributes.Public;
            // Declares a parameter of type string named "TestStringParameter".    

            //baseStringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "TestStringParameter"));

            // Calls a base class constructor with the TestStringParameter parameter.
            baseStringConstructor.BaseConstructorArgs.Add(new CodePrimitiveExpression("TestStringParameter"));
            // Adds the constructor to the Members collection of the DerivedType.
            DerivedType.Members.Add(baseStringConstructor);


            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "Hello";
            method.Attributes = MemberAttributes.Public;
            //method.ReturnType = new CodeTypeReference("System.String");
            //method.Parameters.Add(new CodeParameterDeclarationExpression("System.String", "inputMessage"));
            //method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("inputMessage")));
            //DerivedType.Members.Add(method);
            CodeParameterDeclarationExpression arg = new CodeParameterDeclarationExpression(typeof(string), "inputMessage");
            method.Parameters.Add(arg);
            method.ReturnType = new CodeTypeReference(typeof(string));
            CodeSnippetStatement methodBody = new CodeSnippetStatement(scriptBody);
            method.Statements.Add(methodBody);
            DerivedType.Members.Add(method);


            //// 创建新的类声明
            //CodeTypeDeclaration parentClass = new CodeTypeDeclaration(typeName);
            //nameSpace.Types.Add(parentClass);

            //// 构造函数
            ////CodeConstructor constructor = new CodeConstructor();
            ////constructor.Attributes = MemberAttributes.Public;
            ////parentClass.Members.Add(constructor);

            //CodeTypeDeclaration derivedType = new CodeTypeDeclaration("DerivedType");
            //derivedType.BaseTypes.Add(new CodeTypeReference("Hello"));
            //nameSpace.Types.Add(derivedType);

            ////constructor.BaseConstructorArgs

            ////方法
            //CodeMemberMethod method = new CodeMemberMethod();
            //method.Name = "Hello";
            //method.Attributes = MemberAttributes.Public;
            //CodeParameterDeclarationExpression arg = new CodeParameterDeclarationExpression(typeof(string), "inputMessage");
            //method.Parameters.Add(arg);
            //method.ReturnType = new CodeTypeReference(typeof(string));
            //// 添加方法实体需要的代码
            //CodeSnippetStatement methodBody = new CodeSnippetStatement(scriptBody);
            //method.Statements.Add(methodBody);
            //parentClass.Members.Add(method);
            return unit;

        }

        /// <summary>
        /// 代码构建类
        /// </summary>
        /// <param name="typeNamespace"></param>
        /// <param name="typeName"></param>
        /// <param name="scriptBody"></param>
        /// <returns></returns>
        public string GenerateCode(string typeNamespace,
            string typeName, string scriptBody)
        {
            // 调用我们前面的方法创建CodeCompileUnit
            CodeCompileUnit ccu = CreateExecutionClass(typeNamespace, typeName, scriptBody);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = false;
            options.IndentString = "\t"; //指定缩进字符
            options.BracingStyle = "C"; //大括号换行

            StringWriter sw = new StringWriter();
            try
            {
                provider.GenerateCodeFromCompileUnit(ccu, sw, options);
                sw.Flush();
            }
            finally
            {
                sw.Close();
            }
            return sw.GetStringBuilder().ToString();
        }
    }

    //public class ProcedureDataAccess
    //{
    //    private string par = string.Empty;
    //    public ProcedureDataAccess(string param)
    //    {
    //        par = param;
    //    }
    //}
}
