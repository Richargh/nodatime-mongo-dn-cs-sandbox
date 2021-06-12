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
    public class MongoPizzas_InstantTest : IAsyncLifetime
    {
        private readonly IInstantPizzas _testling;
        private readonly TestcontainersContainer _mongoContainer;
        
        public MongoPizzas_InstantTest()
        {
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
            var pizza = InstantPizza.Now(InstantPizzaId.Random());
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.InstantUtc.ToUnixTimeMilliseconds().Should().Be(pizza.InstantUtc.ToUnixTimeMilliseconds());
            // result!.InstantUtc.Should().Be(pizza.InstantUtc); // cannot assert this because some nanosecond precision is lost after deserialization
        }
    }
}