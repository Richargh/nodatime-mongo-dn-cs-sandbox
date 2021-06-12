using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    public class MongoPizzasTest
    {
        private const string MongoUrl = "mongodb://localhost:27017";
        
        [Fact(DisplayName = "Should be able to retrieve the pizza that was added")]
        public async Task AddOnePizza()
        {
            // given
            var pizza = new Pizza(PizzaId.Random());
            var testling = new MongoPizzas(new MongoUrl(MongoUrl));
            // when
            await testling.Put(pizza);
            // then
            var result = await testling.FindById(pizza.Id);
            result.Should().NotBeNull();
        }
        
        [Fact(DisplayName = "Should not be able to retrieve a Pizza with an unknown id")]
        public async Task UnknownId()
        {
            // given
            var testling = new MongoPizzas(new MongoUrl(MongoUrl));
            // when
            // then
            var result = await testling.FindById(PizzaId.Random());
            result.Should().BeNull();
        }
    }
}