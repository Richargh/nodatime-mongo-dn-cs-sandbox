using System.Threading.Tasks;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public interface IPizzas
    {
        public Task Put(Pizza pizza);

        public Task<Pizza?> FindById(PizzaId id);
    }
}