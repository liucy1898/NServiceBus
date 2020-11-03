﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace NServiceBus.AcceptanceTests.Core.FakeTransport.ProcessingOptimizations
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using AcceptanceTesting.Customization;
    using EndpointTemplates;
    using NUnit.Framework;
    using Transport;

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

                EndpointSetup(template, (endpointConfiguration, _) => endpointConfiguration.UseTransport(new FakeTransport()));
            }
        }

        class FakeReceiver : IMessageReceiver
        {
            private readonly ReceiveSettings receveSettings;

            public FakeReceiver(ReceiveSettings receveSettings, string id)
            {
                this.receveSettings = receveSettings;
            }

            public Task StartReceive(PushRuntimeSettings limitations, Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CancellationToken cancellationToken)
            {
                // The LimitMessageProcessingConcurrencyTo setting only applies to the input queue
                if (receveSettings.ReceiveAddress == Conventions.EndpointNamingConvention(typeof(ThrottledEndpoint)))
                {
                    Assert.AreEqual(10, limitations.MaxConcurrency);
                }

                return Task.CompletedTask;
            }

            public Task StopReceive(CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }

            public IManageSubscriptions Subscriptions { get; }

            public string Id => receveSettings.Id;

        }

        class FakeDispatcher : IMessageDispatcher
        {
            public Task Dispatch(TransportOperations outgoingMessages, TransportTransaction transaction, CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
        }

        class FakeTransport : TransportDefinition
        {
            public override Task<TransportInfrastructure> Initialize(Settings settings, ReceiveSettings[] receivers, string[] SendingAddresses, CancellationToken cancellationToken)
            {
                return Task.FromResult<TransportInfrastructure>(new FakeTransportInfrastructure(receivers));
            }

            public override string ToTransportAddress(EndpointAddress address)
            {
                return address.ToString();
            }

            public override IReadOnlyCollection<TransportTransactionMode> SupportedTransactionModes { get; protected set; } = new[]
            {
                TransportTransactionMode.None
            };

            public override bool SupportsTTBR { get; } = false;

            public override bool SupportsDelayedDelivery { get; } = true;

            public override bool SupportsPublishSubscribe { get; } = true;
        }

        class FakeTransportInfrastructure : TransportInfrastructure
        {
            public FakeTransportInfrastructure(ReceiveSettings[] receiveSettingses)
            {
                Receivers = receiveSettingses.Select<ReceiveSettings, IMessageReceiver>(s => new FakeReceiver(s, s.Id))
                    .ToList().AsReadOnly();
            }

            public override IMessageDispatcher Dispatcher => new FakeDispatcher();
            public override ReadOnlyCollection<IMessageReceiver> Receivers { get; protected set; }

            public override ValueTask DisposeAsync()
            {
                return new ValueTask();
            }
        }
    }
}
