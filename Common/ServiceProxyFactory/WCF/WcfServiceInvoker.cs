using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace Common.ServiceProxyFactory.WCF
{
    public class WcfServiceInvoker
    {
        private static Dictionary<string, ChannelFactory> channelFactories = new Dictionary<string, ChannelFactory>();
        private static object syncHelper = new object();

        private static ChannelFactory<TChannel> GetChannelFactory<TChannel>(string endpointConfigurationName)
        {
            ChannelFactory<TChannel> channelFactory = null;
            if (channelFactories.ContainsKey(endpointConfigurationName))
            {
                channelFactory = channelFactories[endpointConfigurationName] as ChannelFactory<TChannel>;
            }

            if (null == channelFactory)
            {
                channelFactory = new ChannelFactory<TChannel>(endpointConfigurationName);
                lock (syncHelper)
                {
                    channelFactories[endpointConfigurationName] = channelFactory;
                }
            }
            return channelFactory;
        }

        public static void Invoke<TChannel>(Action<TChannel> action, TChannel proxy)
        {
            ICommunicationObject channel = proxy as ICommunicationObject;
            if (null == channel)
            {
                throw new ArgumentException("The proxy is not a valid channel implementing the ICommunicationObject interface", "proxy");
            }
            try
            {
                action(proxy);
            }
            catch (TimeoutException)
            {
                channel.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                channel.Abort();
                throw;
            }
            finally
            {
                channel.Close();
            }
        }

        public static TResult Invoke<TChannel, TResult>(Func<TChannel, TResult> function, TChannel proxy)
        {
            var channel = proxy as ICommunicationObject;
            if (null == channel)
            {
                throw new ArgumentException("The proxy is not a valid channel implementing the ICommunicationObject interface", "proxy");
            }
            try
            {
                return function(proxy);
            }
            catch (TimeoutException)
            {
                channel.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                channel.Abort();
                throw;
            }
            finally
            {
                channel.Close();
            }
        }

        public static void Invoke<TChannel>(Action<TChannel> action, string endpointConfigurationName)
        {
            Guard.ArgumentNotNullOrEmpty(endpointConfigurationName, "endpointConfigurationName");
            Invoke(action, GetChannelFactory<TChannel>(endpointConfigurationName).CreateChannel());
        }

        public static TResult Invoke<TChannel, TResult>(Func<TChannel, TResult> function,
            string endpointConfigurationName)
        {
            Guard.ArgumentNotNullOrEmpty(endpointConfigurationName, "endpointConfigurationName");
            return Invoke(function, GetChannelFactory<TChannel>(endpointConfigurationName).CreateChannel());
        }
    }


    public class WcfServiceInvoker<TChannel> : WcfServiceInvoker
    {
        public string EndpointConfigurationName
        { get; private set; }

        public WcfServiceInvoker(string endpointConfigurationName)
        {
            Guard.ArgumentNotNullOrEmpty(endpointConfigurationName, "endpointConfigurationName");
            this.EndpointConfigurationName = endpointConfigurationName;
        }

        public void Invoke(Action<TChannel> action)
        {
            Invoke(action, this.EndpointConfigurationName);
        }

        public TResult Invoke<TResult>(Func<TChannel, TResult> function)
        {
            return Invoke(function, this.EndpointConfigurationName);
        }
    }

    public class ServiceProxyBase<TChannel>
    {
        public virtual WcfServiceInvoker<TChannel> Invoker
        { get; private set; }

        public ServiceProxyBase(string endpointConfigurationName)
        {
            Guard.ArgumentNotNullOrEmpty(endpointConfigurationName, "endpointConfigurationName");
            this.Invoker = new WcfServiceInvoker<TChannel>(endpointConfigurationName);
        }
    }
}
