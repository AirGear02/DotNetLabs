using Lab2.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    public class Option : IIdentifiable, IReadableFromString
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [InverseProperty("Option")]
        public virtual ICollection<Order> Orders { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            Title = values[0];
            Description = values[1];
            Price = Convert.ToDecimal(values[2]);
        }

        public override string ToString()
        {
            return $"Id: {Id}; Title: {Title}; Description: {Description}; Price: {Price}";
        }
    }
}
