﻿namespace NServiceBus.AcceptanceTests.Core.FakeTransport.ProcessingOptimizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using AcceptanceTesting.Customization;
    using EndpointTemplates;
    using Extensibility;
    using NServiceBus.Routing;
    using NUnit.Framework;
    using Transport;
    using CriticalError = NServiceBus.CriticalError;

    public class When_using_concurrency_limit : NServiceBusAcceptanceTest
    {
        [Test]
        public Task Should_pass_it_to_the_transport()
        {
            return Scenario.Define<ScenarioContext>()
                .WithEndpoint<ThrottledEndpoint>(b => b.CustomConfig(c => c.LimitMessageProcessingConcurrencyTo(10)))
                .Done(c => c.EndpointsStarted)
                .Run();

            //Assert in FakeReceiver.Start
        }

        class ThrottledEndpoint : EndpointConfigurationBuilder
        {
            public ThrottledEndpoint()
            {
                var template = new DefaultServer
                {
                    PersistenceConfiguration = new ConfigureEndpointInMemoryPersistence()
                };

                EndpointSetup(template, (endpointConfiguration, _) => endpointConfiguration.UseTransport(new Core.FakeTransport.FakeTransport()));
            }
        }

        class FakeReceiver : IPushMessages
        {
            PushSettings pushSettings;

            public Task Init(Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CriticalError criticalError, PushSettings settings)
            {
                pushSettings = settings;
                return Task.FromResult(0);
            }

            public void Start(PushRuntimeSettings limitations)
            {
                // The LimitMessageProcessingConcurrencyTo setting only applies to the input queue
                if (pushSettings.InputQueue == Conventions.EndpointNamingConvention(typeof(ThrottledEndpoint)))
                {
                    Assert.AreEqual(10, limitations.MaxConcurrency);
                }
            }

            public Task Stop()
            {
                return Task.FromResult(0);
            }
        }

        class FakeQueueCreator : ICreateQueues
        {
            public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
            {
                return Task.FromResult(0);
            }
        }

        class FakeDispatcher : IDispatchMessages
        {
            public Task Dispatch(TransportOperations outgoingMessages, TransportTransaction transaction, ContextBag context)
            {
                return Task.FromResult(0);
            }
        }

        class FakeTransport : TransportDefinition
        {
            public override TransportInfrastructure Initialize(TransportSettings settings)
            {
                return new FakeTransportInfrastructure();
            }
        }

        class FakeTransportInfrastructure : TransportInfrastructure
        {
            public override IEnumerable<Type> DeliveryConstraints { get; } = Enumerable.Empty<Type>();
            public override TransportTransactionMode TransactionMode { get; } = TransportTransactionMode.None;

            public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance)
            {
                return instance;
            }

            public override string ToTransportAddress(LogicalAddress logicalAddress)
            {
                return logicalAddress.ToString();
            }

            public override TransportReceiveInfrastructure ConfigureReceiveInfrastructure(ReceiveSettings receiveSettings)
            {
                return new TransportReceiveInfrastructure(() => new FakeReceiver(), () => new FakeQueueCreator(), () => Task.FromResult(StartupCheckResult.Success));
            }

            public override TransportSendInfrastructure ConfigureSendInfrastructure()
            {
                return new TransportSendInfrastructure(() => new FakeDispatcher(), () => Task.FromResult(StartupCheckResult.Success));
            }

            public override TransportSubscriptionInfrastructure ConfigureSubscriptionInfrastructure(SubscriptionSettings subscriptionSettings)
            {
                throw new NotImplementedException();
            }
        }
    }
}
