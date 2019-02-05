using System.Collections.Generic;
//using System.ComponentModel;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MultiQueue.Core.Ioc;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Queues;
using MultiQueue.Core.Services;
using Rebus.Config;
using Rebus.Routing.TypeBased;


namespace MultiQueue.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private List<IWindsorContainer> _containers;
        
        protected void Application_Start()
        {
            // Main container first
            var mainContainer = CreateContainer();

            // Hook the main container into the web api dependency resolver
            var dependencyResolver = new WindsorDependencyResolver(mainContainer);
            var configuration = GlobalConfiguration.Configuration;
            configuration.DependencyResolver = dependencyResolver;
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(mainContainer.Kernel));

            // Wire up the payment queue to use mainContainer
            var paymentQueue = new PaymentQueue(
                Configure
                    .With(new CastleWindsorContainerAdapter(CreateContainer()))
                    .Transport(t => t.UseSqlServerAsOneWayClient("server=.;database=Scratch;trusted_connection=true"))
                    .Routing(r => r.TypeBased().Map<Payment>("PaymentQueue"))
                    .Start()
            );

            

            // Need another container for the orders queue
            var orderQueue = new OrderQueue(
                Configure
                    .With(new CastleWindsorContainerAdapter(CreateContainer()))
                    .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://localhost"))
                    .Routing(r => r.TypeBased().Map<Order>("OrderQueue"))
                    .Start()
            );

            // Register our custom queues in every container... not sure this is going to work, should
            foreach (var container in _containers)
            {
                container.Register(
                    Component.For<PaymentQueue>().Instance(paymentQueue),
                    Component.For<OrderQueue>().Instance(orderQueue)
                );
            }
            

            

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


        }

        private IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            container.Install(FromAssembly.Containing<IPaymentsService>());
            container.Register(Classes.FromThisAssembly()
                .BasedOn<ApiController>()   
                .LifestylePerWebRequest());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            if (_containers == null)
            {
                _containers = new List<IWindsorContainer>();
            }
            _containers.Add(container);
            return container;
        }

        protected void Application_End()
        {
            foreach (var container in _containers)
            {
                container.Dispose();
            }
            base.Dispose();
        }
    }
}
