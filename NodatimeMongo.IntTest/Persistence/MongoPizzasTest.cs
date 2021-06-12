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
    public class MongoPizzasTest : IAsyncLifetime
    {
        private readonly IDateTimePizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;

        public MongoPizzasTest()
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
            var pizza = DateTimePizza.Now(DateTimePizzaId.Random());
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
    }
}