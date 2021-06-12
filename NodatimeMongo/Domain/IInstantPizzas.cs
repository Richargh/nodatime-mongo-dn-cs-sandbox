using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public interface IInstantPizzas
    {
        public Task Put(InstantPizza instantPizza);

        public Task<InstantPizza?> FindById(InstantPizzaId id);
        
        Task<List<InstantPizza>> FindOlderThan(Instant utcNow);
    }
}