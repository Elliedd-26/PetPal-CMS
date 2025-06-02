using System;
using System.Collections.Generic;

namespace PetPalCMS.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime Birthdate { get; set; }

        // Navigation properties
        public ICollection<VaccinationRecord> VaccinationRecords { get; set; }
        public ICollection<DewormingRecord> DewormingRecords { get; set; }
    }
}
