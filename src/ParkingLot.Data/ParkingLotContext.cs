namespace ParkingLot.Data
{
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ParkingLot> ParkingLot { get; set; }
    }
}
