namespace ParkingLot.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities = ParkingLot.Data.Model;

    public interface IParkingLotFacade
    {
        IEnumerable<Entities.ParkingLot> GetAllParkingLots();

        Entities.ParkingLot GetParkingLot(Guid id);

        void AddParkingLot(Entities.ParkingLot parkingLote);

        void UpdateParkingLot(Entities.ParkingLot parkingLote);

        void DeleteParkingLot(Guid id);
    }
}
