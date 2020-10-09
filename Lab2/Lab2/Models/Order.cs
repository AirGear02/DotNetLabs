using Lab2.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    public class Order : IIdentifiable
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        
        public Guid OptionId { get; set; }
        [ForeignKey("OptionId")]
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
