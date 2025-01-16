using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Domain_Models
{
    public class HealthExamination : BaseEntity 
    {
        public string? Description { get; set; }
        public DateTime DateTaken { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }
        public Guid PolyclinicId { get; set; }
        [JsonIgnore]
        public virtual Polyclinic? Polyclinic { get; set; }

    }
}
