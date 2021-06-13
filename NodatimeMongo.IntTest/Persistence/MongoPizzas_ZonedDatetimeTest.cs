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
    public class MongoPizzas_ZonedDateTimeTest : IAsyncLifetime
    {
        private readonly IClock _clock;
        private readonly IZonedDateTimePizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;
        
        public MongoPizzas_ZonedDateTimeTest()
        {
            _clock = SystemClock.Instance;
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
        public async Task ZonedDateTimeNotNull()
        {
            // given
            var pizza = CreatePizza();
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            // the selected serializer drops milliseconds during serialization, otherwise we'd compare via ZonedDateTime.ToOffsetDateTime()
            // we could fix this by writing our own serializer that uses the extended invariant instead
            result!.DateTime.ToDateTimeUtc().Should().BeCloseTo(pizza.DateTime.ToDateTimeUtc(), TimeSpan.FromSeconds(1));
        }
        
        [Fact(DisplayName = "Should be able to find one Pizza older than one minute ago",
            Skip = "Cannot get this to work. Comparing ZoneDateTimes is not supported.")]
        public async Task OnePizzaOlder()
        {
            // given
            var pizza = CreatePizza();
            await _testling.Put(pizza);
            // when
            var result = await _testling.FindOlderThan(
                new ZonedDateTime(_clock.GetCurrentInstant() - Duration.FromMinutes(1), DateTimeZone.Utc));
            // then
            result.Select(x => x.Id).Should().HaveCount(1).And.Contain(pizza.Id);
        }
        
        [Fact(DisplayName = "Should not be able to find one Pizza older than the future",
            Skip = "Cannot get this to work. Comparing ZoneDateTimes is not supported.")]
        public async Task NoPizzaOlderthanFuture()
        {
            // given
            var pizza = CreatePizza();
            await _testling.Put(pizza);
            // when
            var result = await _testling.FindOlderThan(
                new ZonedDateTime(_clock.GetCurrentInstant() + Duration.FromMinutes(1), DateTimeZone.Utc));
            // then
            result.Select(x => x.Id).Should().HaveCount(0);
        }

        private ZonedDateTimePizza CreatePizza() 
            => new ZonedDateTimePizza(
                ZonedDateTimePizzaId.Random(),
                new ZonedDateTime(_clock.GetCurrentInstant(), DateTimeZone.Utc));
    }
}