using System;
using MessagePack;

namespace RestaurantB.Models
{
    [MessagePackObject]
    public class Car
    {
        [Key(0)]
        public int Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public int Year { get; set; } 
    }
}