using Lab3.Interfaces;
using System;
using System.Collections.Generic;

namespace Lab3.Models
{
    public class Option : IPrintableToHtmlRow
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

   
        public override string ToString()
        {
            return $"Id: {Id}; Title: {Title}; Description: {Description}; Price: {Price}";
        }

        public string PrintToHtmlRow()
        {
            return $"<tr><td>{Title}</td><td>{Description}</td><td>{Price}</td></tr>";
        }
    }
}
