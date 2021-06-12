using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;

namespace Richargh.Sandbox.NodatimeMongo.Persistence
{
    public class MongoDateTimePizzas : IDateTimePizzas
    {
        private readonly IMongoCollection<DateTimePizza> _allPizzas;
        private static readonly ReplaceOptions Overwrite = new ReplaceOptions {IsUpsert = true};
        
        public MongoDateTimePizzas(MongoUrl mongoUrl)
        {
            _allPizzas = Database.Create(mongoUrl).GetCollection<DateTimePizza>("sandbox.nodatimemongo.pizzas.datetime");
        }
        
        public async Task Put(DateTimePizza dateTimePizza)
        {
            await _allPizzas.ReplaceOneAsync(x => dateTimePizza.Id.Equals(x.Id), dateTimePizza, Overwrite);
        }
        
        public async Task<DateTimePizza?> FindById(DateTimePizzaId id)
        {
            return (await _allPizzas.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
        }

        public Task<List<DateTimePizza>> FindOlderThan(DateTime utcNow)
        {
            return _allPizzas.Find(x => x.DateTimeUtc > utcNow).ToListAsync();
        }
    }
}