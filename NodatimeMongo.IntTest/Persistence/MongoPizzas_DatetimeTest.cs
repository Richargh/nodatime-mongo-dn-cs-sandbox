using System;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;
using Richargh.Sandbox.NodatimeMongo.Persistence;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Persistence
{
    [Collection(MongoCollection.Name)]
    public class MongoPizzas_DateTimeTest
    {
        private readonly IPizzas _testling;
        public MongoPizzas_DateTimeTest()
        {
            _testling = new MongoPizzas(new MongoUrl(MongoCollection.MongoUrl));
        }
        
        [Fact(DisplayName = "Should be able to deserialize Utc Datetime")]
        public async Task UtcDateTimeNotNull()
        {
            // given
            var pizza = Pizza.Now(PizzaId.Random());
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.DateTimeUtc.Should().BeCloseTo(pizza.DateTimeUtc); // mongo drops some precision
        }
        
        [Fact(
            DisplayName = "Should be able to deserialize Local Datetime",
            Skip = "This does not work. By default mongo serializes dates as Utc and does not make them local again on deserialize")]
        public async Task LocalDateTimeNotNull()
        {
            // given
            var pizza = Pizza.Now(PizzaId.Random());
            // when
            await _testling.Put(pizza);
            // then
            var result = await _testling.FindById(pizza.Id);
            result!.DateTimeLocal.Should().BeCloseTo(pizza.DateTimeLocal);
        }
    }
}