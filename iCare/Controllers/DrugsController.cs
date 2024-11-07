using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Linq;
using iCare.Models;
using System.Collections.Generic;

namespace iCare.Controllers
{
    [Route("api/[controller]")]
    public class DrugsController : Controller
    {
        private readonly List<Drug> _drugData;

        public DrugsController()
        {
            // Load the drug reference data from the JSON file
            var drugDataPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "drugdata", "drug_reference.json");

            try
            {
                var jsonData = System.IO.File.ReadAllText(drugDataPath);
                _drugData = JsonSerializer.Deserialize<List<Drug>>(jsonData) ?? new List<Drug>();

                // Add this line to output the count of drugs loaded
                Console.WriteLine($"Drug Data Count: {_drugData.Count}");
            }
            catch (FileNotFoundException)
            {
                // Log or handle the case where the file is not found
                _drugData = new List<Drug>(); // Initialize an empty list if the file is missing
                Console.WriteLine("Drug reference file not found.");
            }
            catch (JsonException)
            {
                // Handle JSON parsing errors
                _drugData = new List<Drug>(); // Initialize an empty list if JSON is malformed
                Console.WriteLine("Error parsing drug reference file.");
            }
        }

        [HttpGet("search")]
        public IActionResult SearchDrug(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query cannot be empty.");
            }

            Console.WriteLine("Starting search for drug with query: " + query);

            // Debugging: Print each drug name in the dataset
            foreach (var drug in _drugData)
            {
                Console.WriteLine($"Available drug: {drug.Name}");
            }

            var result = _drugData.FirstOrDefault(d =>
                d.Name != null &&
                d.Name.Equals(query, StringComparison.OrdinalIgnoreCase)
            );

            if (result != null)
            {
                Console.WriteLine("Drug found: " + result.Name);
                return Json(result); // Return JSON with the drug data
            }
            else
            {
                Console.WriteLine("Drug not found for query: " + query);
                return NotFound("Drug not found.");
            }
        }
    }
}





