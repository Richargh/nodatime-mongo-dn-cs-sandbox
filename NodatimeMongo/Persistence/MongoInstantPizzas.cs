using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using NodaTime;
using Richargh.Sandbox.NodatimeMongo.Domain;

namespace Richargh.Sandbox.NodatimeMongo.Persistence
{
    public class MongoInstantPizzas : IInstantPizzas
    {
        private readonly IMongoCollection<InstantPizza> _allPizzas;
        private static readonly ReplaceOptions Overwrite = new ReplaceOptions {IsUpsert = true};
        
        public MongoInstantPizzas(MongoUrl mongoUrl)
        {
            _allPizzas = Database.Create(mongoUrl).GetCollection<InstantPizza>("sandbox.nodatimemongo.pizzas.instant");
        }
        
        public async Task Put(InstantPizza instantPizza)
        {
            await _allPizzas.ReplaceOneAsync(x => instantPizza.Id.Equals(x.Id), instantPizza, Overwrite);
        }
        
        public async Task<InstantPizza?> FindById(InstantPizzaId id)
        {
            return (await _allPizzas.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
        }

        public Task<List<InstantPizza>> FindOlderThan(Instant instant)
        {
            return _allPizzas.Find(x => x.Instant > instant).ToListAsync();
        }
    }
}