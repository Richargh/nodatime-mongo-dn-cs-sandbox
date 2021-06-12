using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using FluentAssertions;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    [Collection(MongoCollection.Name)]
    public class MongoPizzas_ZonedDateTimeTest : IAsyncLifetime
    {
        private readonly IZonedDateTimePizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;
        
        public MongoPizzas_ZonedDateTimeTest()
        {
            var testcontainersBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mongo")
                .WithName("mongo")
                .WithPortBinding(MongoCollection.MongoPort);
            _mongoContainer = testcontainersBuilder.Build();
            _testling = new MongoZonedDateTimePizzas(new MongoUrl(MongoCollection.MongoUrl));
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact(DisplayName = "Should be able to deserialize ZonedDatetime")]
        public async Task UtcDateTimeNotNull()
        {
            // given
            var pizza = ZonedDateTimePizza.Now(ZonedDateTimePizzaId.Random());
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            // mongo drops milliseconds during deserialization
            result!.DateTime.ToDateTimeUtc().Should().BeCloseTo(pizza.DateTime.ToDateTimeUtc(), TimeSpan.FromSeconds(1));
        }
        
    }
}