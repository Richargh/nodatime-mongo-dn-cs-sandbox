using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    [Collection(MongoCollection.Name)]
    public class MongoPizzasTest
    {
        private readonly IPizzas _testling;
        public MongoPizzasTest()
        {
            _testling = new MongoPizzas(new MongoUrl(MongoCollection.MongoUrl));
        }
        
        [Fact(DisplayName = "Should be able to retrieve the pizza that was added")]
        public async Task AddOnePizza()
        {
            // given
            var pizza = Pizza.Now(PizzaId.Random());
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
            var result = await _testling.FindById(PizzaId.Random());
            // then
            result.Should().BeNull();
        }
    }
}