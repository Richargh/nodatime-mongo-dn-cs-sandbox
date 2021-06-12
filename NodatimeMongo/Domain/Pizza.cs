using System;

namespace Richargh.Sandbox.NodatimeMongo.Domain
{
    public class Pizza
    {
        public PizzaId Id { get; }

        public DateTime DateTimeUtc { get; }
        public DateTime DateTimeLocal { get; }

        public Pizza(
            PizzaId id, 
            DateTime dateTimeUtc, 
            DateTime dateTimeLocal)
        {
            Id = id;
            
            DateTimeUtc = dateTimeUtc;
            DateTimeLocal = dateTimeLocal;
        }

        public static Pizza Now(PizzaId id)
            => new Pizza(
                id,
                DateTime.UtcNow,
                DateTime.UtcNow.ToLocalTime());
    }
}