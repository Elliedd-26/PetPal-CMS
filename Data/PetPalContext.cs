using Microsoft.EntityFrameworkCore;
using PetPalCMS.Models;

namespace PetPalCMS.Data
{
    public class PetPalContext : DbContext
    {
        public PetPalContext(DbContextOptions<PetPalContext> options) : base(options) {}

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<VaccinationRecord> VaccinationRecords { get; set; }
        public DbSet<DewormingRecord> DewormingRecords { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
