using Lab2.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models
{
    public class Order : IIdentifiable
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
