using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Richargh.Sandbox.NodatimeMongo.Domain;

namespace Richargh.Sandbox.NodatimeMongo.Persistence
{
    public class MongoPizzas : IPizzas
    {
        private readonly IMongoCollection<Pizza> _allPizzas;
        private static readonly ReplaceOptions Overwrite = new ReplaceOptions {IsUpsert = true};
        
        public MongoPizzas(MongoUrl mongoUrl)
        {
            _allPizzas = Database.Create(mongoUrl).GetCollection<Pizza>("sandbox.nodatimemongo.pizzas");
        }
        
        public async Task Put(Pizza pizza)
        {
            await _allPizzas.ReplaceOneAsync(x => pizza.Id.Equals(x.Id), pizza, Overwrite);
        }
        
        public async Task<Pizza?> FindById(PizzaId id)
        {
            return (await _allPizzas.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
        }

        public Task<List<Pizza>> FindOlderThan(DateTime utcNow)
        {
            return _allPizzas.Find(x => x.DateTimeUtc > utcNow).ToListAsync();
        }
    }
}