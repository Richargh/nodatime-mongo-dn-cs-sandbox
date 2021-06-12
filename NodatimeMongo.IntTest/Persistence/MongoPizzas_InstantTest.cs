using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using FluentAssertions;
using MongoDB.Driver;
using NodaTime;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    [Collection(MongoCollection.Name)]
    public class MongoPizzas_InstantTest : IAsyncLifetime
    {
        private readonly IClock _clock;
        private readonly IInstantPizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;
        
        public MongoPizzas_InstantTest()
        {
            _clock = SystemClock.Instance;
            var testcontainersBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mongo")
                .WithName("mongo")
                .WithPortBinding(MongoCollection.MongoPort);
            _mongoContainer = testcontainersBuilder.Build();
            _testling = new MongoInstantPizzas(new MongoUrl(MongoCollection.MongoUrl));
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }

        [Fact(DisplayName = "Should be able to deserialize Utc Instant")]
        public async Task UtcInstantNotNull()
        {
            // given
            var pizza = CreatePizza();
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.InstantUtc.ToUnixTimeMilliseconds().Should().Be(pizza.InstantUtc.ToUnixTimeMilliseconds());
            // result!.InstantUtc.Should().Be(pizza.InstantUtc); // cannot assert this because some nanosecond precision is lost after deserialization
        }
        
        [Fact(DisplayName = "Should be able to find one Pizza older than one minute ago")]
        public async Task OnePizzaOlder()
        {
            // given
            var pizza = CreatePizza();
            await _testling.Put(pizza);
            // when
            var result = await _testling.FindOlderThan(
                Instant.FromDateTimeUtc(DateTime.UtcNow) - Duration.FromMinutes(1));
            // then
            result.Select(x => x.Id).Should().HaveCount(1).And.Contain(pizza.Id);
        }
        
        [Fact(DisplayName = "Should not be able to find one Pizza older than the future")]
        public async Task NoPizzaOlderthanFuture()
        {
            // given
            var pizza = CreatePizza();
            await _testling.Put(pizza);
            // when
            var result = await _testling.FindOlderThan(
                Instant.FromDateTimeUtc(DateTime.UtcNow) + Duration.FromMinutes(1));
            // then
            result.Select(x => x.Id).Should().HaveCount(0);
        }

        private InstantPizza CreatePizza() 
            => new InstantPizza(
                InstantPizzaId.Random(),
                _clock.GetCurrentInstant());
    }
}