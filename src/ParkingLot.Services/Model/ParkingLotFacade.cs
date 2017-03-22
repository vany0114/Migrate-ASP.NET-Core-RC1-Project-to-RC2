namespace ParkingLot.Services.Model
{
    using Interfaces;
    using ParkingLot.Data.Interfaces;
    using ParkingLot.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities = ParkingLot.Data.Model;

    public class ParkingLotFacade : IParkingLotFacade
    {
        private readonly IRepository<Entities.ParkingLot> _repository;

        public ParkingLotFacade(IRepository<Entities.ParkingLot> repository)
        {
            _repository = repository;
        }

        public void AddParkingLot(Entities.ParkingLot parkingLote)
        {
            _repository.Insert(parkingLote);
        }

        public void DeleteParkingLot(Guid id)
        {
            _repository.Delete(id);
        }

        public IEnumerable<Entities.ParkingLot> GetAllParkingLots()
        {
            return _repository.List();
        }

        public Entities.ParkingLot GetParkingLot(Guid id)
        {
            return _repository.GetById(id);
        }

        public void UpdateParkingLot(Entities.ParkingLot parkingLote)
        {
            _repository.Update(parkingLote);
        }
    }
}
