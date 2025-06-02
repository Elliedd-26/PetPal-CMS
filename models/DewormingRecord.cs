using System;

namespace PetPalCMS.Models
{
    public class DewormingRecord
    {
        public int DewormingRecordId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string DewormingProduct { get; set; }
        public DateTime DewormingDate { get; set; }

        // Navigation properties
        public Pet Pet { get; set; }
        public Vet Vet { get; set; }
    }
}
