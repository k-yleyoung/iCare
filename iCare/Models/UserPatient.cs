using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Models
{
    public class UserPatient
    {
        // id of the worker (doctor or nurse) assigned to the patient
        public int WorkerId { get; set; }

        // navigation property linking to the user (worker), nullable to allow unassigned workers
        public User? User { get; set; }

        // id of the patient assigned to the worker
        public int PatientId { get; set; }

        // navigation property linking to the patient, nullable to allow unassigned patients
        public Patient? Patient { get; set; }

        // timestamp indicating when the patient was assigned to the worker, defaulting to the current time
        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
