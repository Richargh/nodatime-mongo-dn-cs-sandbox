using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public interface IZonedDateTimePizzas
    {
        public Task Put(ZonedDateTimePizza dateTimePizza);

        public Task<ZonedDateTimePizza?> FindById(ZonedDateTimePizzaId id);

        public Task<List<ZonedDateTimePizza>> FindOlderThan(ZonedDateTime zonedDateTime);
    }
}