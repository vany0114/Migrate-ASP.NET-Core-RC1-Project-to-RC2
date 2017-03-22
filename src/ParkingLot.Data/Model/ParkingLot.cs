namespace ParkingLot.Data.Model
{
    using Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ParkingLot")]
    public partial class ParkingLot : IEntity
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int PrivateSpaces { get; set; }

        public int PublicSpaces { get; set; }

        public int BikeSpaces { get; set; }

        public int Gates { get; set; }
    }
}
