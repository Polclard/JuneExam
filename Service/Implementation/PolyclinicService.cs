using Domain.Domain_Models;
using Microsoft.Extensions.Logging;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class PolyclinicService : IPolyclinicService
    {
        private readonly IRepository<Polyclinic> _polyclinicRepository;

        public PolyclinicService(IRepository<Polyclinic> polyclinicRepository)
        {
            _polyclinicRepository = polyclinicRepository;
        }

        public void CreateNewPolyclinic(Polyclinic p)
        {
            _polyclinicRepository.Insert(p);
        }

        public void DeletePolyclinic(Guid id)
        {
            _polyclinicRepository.Delete(GetDetailsForPolyclinic(id));
        }

        public List<Polyclinic> GetAllPolyclinics()
        {
            return _polyclinicRepository.GetAll().ToList();
        }

        public Polyclinic GetDetailsForPolyclinic(Guid? id)
        {
            return _polyclinicRepository.Get(id);
        }

        public void UpdateExistingPolyclinic(Polyclinic p)
        {
            _polyclinicRepository.Update(p);
        }
    }
}
