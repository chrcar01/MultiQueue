using Castle.Windsor;
using Castle.Windsor.Installer;
using MultiQueue.Core.Repositories;
using Rebus.Config;
using Rebus.Retry.Simple;
using System;
using Castle.MicroKernel.Registration;
using MultiQueue.Core.Messages;
using MultiQueue.Core.Queues;
using MultiQueue.OrderConsumer.Handlers;
using Rebus.CastleWindsor;
using Rebus.Routing.TypeBased;

namespace MultiQueue.OrderConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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

                var mainContainer = CreateContainer();
                mainContainer.Register(Component.For<PaymentQueue>().Instance(paymentQueue), Component.For<OrderQueue>().Instance(orderQueue));
                var bus = Configure
                    .With(new CastleWindsorContainerAdapter(mainContainer))
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseRabbitMq("amqp://localhost", "OrderQueue"))
                    .Options(o =>
                    {
                        o.SimpleRetryStrategy(errorQueueAddress: "OrderQueue-error");
                        o.SetMaxParallelism(1);
                        o.SetNumberOfWorkers(1);
                    })
                    .Start();

                while (true)
                {
                    var input = Console.ReadLine();
                    if (input == "q")
                    {
                        bus.Dispose();
                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static WindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.Containing<IOrdersRepository>());
            container.AutoRegisterHandlersFromAssembly(typeof(OrderHandler).Assembly);
            return container;
        }
    }
}
