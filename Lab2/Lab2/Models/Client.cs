using Lab2.Interfaces;
using System;
using System.Collections.Generic;

namespace Lab2.Models
{
    public class Client : IIdentifiable, IReadableFromString
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            
            Name = values[0];
            Surname = values[1];
            MiddleName = values[2];
            Address = values[3];
            PhoneNumber = values[4];
        }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; Surname: {Surname}; MiddleName: {MiddleName}; Address: {Address}; PhoneNumber: {PhoneNumber};";
        }
    }
}
