using System;

namespace PetPalCMS.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int PetId { get; set; }
        public string Message { get; set; }
        public DateTime NotifyDate { get; set; }

        // Navigation property
        public Pet Pet { get; set; }
    }
}
