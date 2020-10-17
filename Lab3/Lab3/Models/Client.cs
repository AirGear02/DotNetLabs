using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Client 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public virtual  ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; Surname: {Surname}; MiddleName: {MiddleName}; Address: {Address}; PhoneNumber: {PhoneNumber};";
        }
    }
}
