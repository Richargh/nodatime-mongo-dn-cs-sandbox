namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public class Pizza
    {
        public PizzaId Id { get; }

        public Pizza(PizzaId Id)
        {
            this.Id = Id;
        }
    }
}