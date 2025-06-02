using System.Collections.Generic;

namespace PetPalCMS.Models
{
    public class Vet
    {
        public int VetId { get; set; }
        public string Name { get; set; }
        public string ClinicName { get; set; }
        public string ContactInfo { get; set; }

        // Navigation properties (optional)
        public ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public ICollection<DewormingRecord> DewormingRecords { get; set; }
    }
}
