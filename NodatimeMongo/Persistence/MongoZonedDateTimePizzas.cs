using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using NodaTime;
using Richargh.Sandbox.NodatimeMongo.Domain;

namespace Richargh.Sandbox.NodatimeMongo.Persistence
{
    public class MongoZonedDateTimePizzas : IZonedDateTimePizzas
    {
        private readonly IMongoCollection<ZonedDateTimePizza> _allPizzas;
        private static readonly ReplaceOptions Overwrite = new ReplaceOptions {IsUpsert = true};
        
        public MongoZonedDateTimePizzas(MongoUrl mongoUrl)
        {
            _allPizzas = Database.Create(mongoUrl).GetCollection<ZonedDateTimePizza>("sandbox.nodatimemongo.pizzas.zoneddatetime");
        }
        
        public async Task Put(ZonedDateTimePizza dateTimePizza)
        {
            await _allPizzas.ReplaceOneAsync(x => dateTimePizza.Id.Equals(x.Id), dateTimePizza, Overwrite);
        }
        
        public async Task<ZonedDateTimePizza?> FindById(ZonedDateTimePizzaId id)
        {
            return (await _allPizzas.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
        }

        public Task<List<ZonedDateTimePizza>> FindOlderThan(ZonedDateTime utcNow)
        {
            // cannot get the following to work. Comparing ZoneDateTimes is not supported. Probably because they are represented as strings internally
            Builders<ZonedDateTimePizza>.Filter.Lt(x => x.DateTime, utcNow);
            var comparer = ZonedDateTime.Comparer.Instant;
            return _allPizzas.Find(x => comparer.Compare(x.DateTime, utcNow) < 0).ToListAsync();
        }
    }
}