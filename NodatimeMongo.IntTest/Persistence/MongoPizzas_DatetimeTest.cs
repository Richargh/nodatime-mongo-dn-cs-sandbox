using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using FluentAssertions;
using FluentAssertions.Extensions;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    [Collection(MongoCollection.Name)]
    public class MongoPizzas_DateTimeTest : IAsyncLifetime
    {
        private readonly IDateTimePizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;
        
        public MongoPizzas_DateTimeTest()
        {
            var testcontainersBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("mongo")
                .WithName("mongo")
                .WithPortBinding(MongoCollection.MongoPort);
            _mongoContainer = testcontainersBuilder.Build();
            _testling = new MongoDateTimePizzas(new MongoUrl(MongoCollection.MongoUrl));
        }

        public async Task InitializeAsync()
        {
            await _mongoContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _mongoContainer.DisposeAsync();
        }
        
        [Fact(DisplayName = "Should be able to retrieve the pizza that was added")]
        public async Task AddOnePizza()
        {
            // given
            var pizza = CreatePizza();
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result.Should().NotBeNull();
        }
        
        [Fact(DisplayName = "Should not be able to retrieve a Pizza with an unknown id")]
        public async Task UnknownId()
        {
            // given
            // when
            var result = await _testling.FindById(DateTimePizzaId.Random());
            // then
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Should be able to deserialize Utc Datetime")]
        public async Task UtcDateTimeNotNull()
        {
            // given
            var pizza = CreatePizza();
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.DateTimeUtc.Should().BeCloseTo(pizza.DateTimeUtc); // mongo drops some precision
        }
        
        [Fact(
            DisplayName = "Should be able to deserialize Local Datetime")]
        public async Task LocalDateTimeNotNull()
        {
            // given
            var pizza = CreatePizza();
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.DateTimeLocal.Should().BeCloseTo(pizza.DateTimeLocal);
        }
        
        [Fact(DisplayName = "Should be able to find one Pizza older than one minute ago")]
        public async Task OnePizzaOlder()
        {
            // given
            var pizza = CreatePizza();
            await _testling.Put(pizza);
            // when
            var result = await _testling.FindOlderThan(DateTime.UtcNow - 1.Minutes());
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
            var result = await _testling.FindOlderThan(DateTime.UtcNow + 1.Minutes());
            // then
            result.Select(x => x.Id).Should().HaveCount(0);
        }

        public static DateTimePizza CreatePizza()
            => new DateTimePizza(
                DateTimePizzaId.Random(),
                DateTime.UtcNow,
                DateTime.UtcNow.ToLocalTime());
        
    }
}