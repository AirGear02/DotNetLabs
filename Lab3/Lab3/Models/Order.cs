using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        
        public Guid OptionId { get; set; }
        public Option Option { get; set; }

        public int Quantity { get; set; }
        
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }


        public override string ToString()
        {
            return $"Id: {Id}; ClientId: {ClientId}; OptionId: {OptionId}; DateStart: {DateStart.ToShortDateString()}; " +
                $"DateFinish: {DateFinish.ToShortDateString()};";
        }


    }
}
