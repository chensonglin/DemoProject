using System.Configuration;
using System.Globalization;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ServiceLoader
    {
        private static readonly TransparentProxyInterceptor injector = new TransparentProxyInterceptor();
        public static IUnityContainer Container
        { get; private set; }

        static ServiceLoader()
        {
            var unitySection = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;
            if (unitySection == null)
            {
                throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "Missing Unity configuration section."));
            }

            Container = new UnityContainer();
            var containerElement = unitySection.Containers.Default;
            containerElement.Configure(Container);
            Container.AddNewExtension<Interception>();
        }

        public static T LoadService<T>()
        {
            Container.Configure<Interception>().SetDefaultInterceptorFor(typeof(T), injector);
            return Container.Resolve<T>();
        }

        public static T LoadService<T>(string serviceName)
        {
            Container.Configure<Interception>().SetDefaultInterceptorFor(typeof(T), injector);
            return Container.Resolve<T>(serviceName);
        }


        public static void Test()
        {
            //实例化一个控制器
            IUnityContainer unityContainer = new UnityContainer();
            //实现注入
            unityContainer.RegisterType<ITest, Test>();
            //unityContainer.RegisterType<IBirdHome, BirdHome>();

            ITest birdHome = unityContainer.Resolve<ITest>();
            birdHome.Write();//.Swallow.Say();
        }
    }

    public interface ITest
    {
        string Write();
    }

    public class Test:ITest
    {
        public string Write()
        {
            return "i am a test";
        }
    }
}
