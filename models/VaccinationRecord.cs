using System;

namespace PetPalCMS.Models
{
    public class VaccinationRecord
    {
        public int VaccinationRecordId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string VaccineName { get; set; }
        public DateTime VaccinationDate { get; set; }

        public Pet Pet { get; set; }
        public Vet Vet { get; set; }
    }
}
