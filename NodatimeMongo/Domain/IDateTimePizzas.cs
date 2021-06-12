using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public interface IDateTimePizzas
    {
        public Task Put(DateTimePizza dateTimePizza);

        public Task<DateTimePizza?> FindById(DateTimePizzaId id);
        Task<List<DateTimePizza>> FindOlderThan(DateTime utcNow);
    }
}