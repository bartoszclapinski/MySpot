using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Framework;

public class ServiceCollectionTests
{
    [Fact]
    public void test()
    {
        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddScoped<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();


        var messenger = serviceProvider.GetRequiredService<IMessenger>();
        messenger.Send();

        var messenger2 = serviceProvider.GetRequiredService<IMessenger>();
        messenger2.Send();

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull();
    }

    private interface IMessenger
    {
        void Send();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send() => Console.WriteLine($"Sending message with id [{_id}]");
    }
}