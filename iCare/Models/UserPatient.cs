using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Models
{
    public class UserPatient
    {
        public int WorkerId { get; set; }
        public User? User { get; set; } // Nullable now

        public int PatientId { get; set; }
        public Patient? Patient { get; set; } // Nullable now

        public DateTime AssignedAt { get; set; } = DateTime.Now; // Property to track assignment time
    }
}
