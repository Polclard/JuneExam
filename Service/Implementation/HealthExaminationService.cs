using Domain.Domain_Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class HealthExaminationService : IHealthExaminationService
    {
        private readonly IRepository<HealthExamination> _healthExaminationRepository;

        public HealthExaminationService(IRepository<HealthExamination> healthExaminationRepository)
        {
            _healthExaminationRepository = healthExaminationRepository;
        }

        public void CreateNewHealthExamination(HealthExamination p)
        {
            _healthExaminationRepository.Insert(p);
        }

        public void DeleteHealthExamination(Guid id)
        {
            _healthExaminationRepository.Delete(GetDetailsForHealthExamination(id));
        }

        public List<HealthExamination> GetAllHealthExaminations()
        {
            return _healthExaminationRepository.GetAll().ToList();
        }

        public HealthExamination GetDetailsForHealthExamination(Guid? id)
        {
            return _healthExaminationRepository.Get(id);
        }

        public void UpdateExistingHealthExamination(HealthExamination p)
        {
            _healthExaminationRepository.Update(p);
        }
    }
}
