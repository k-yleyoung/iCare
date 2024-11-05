// Models/UploadDocumentViewModel.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace iCare.Models
{
    public class UploadDocumentViewModel
    {
        public int PatientId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
