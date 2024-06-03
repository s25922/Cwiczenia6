using Cw6.Models;
using Cw6.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cw6.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionController : ControllerBase
    {
        private readonly S25922Context _context;

        public PrescriptionController(S25922Context context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(PrescriptionDto prescriptionDto)
        {
            // Check if DueDate >= Date
            if (prescriptionDto.DueDate < prescriptionDto.Date)
            {
                return BadRequest("DueDate must be greater than or equal to Date.");
            }

            // Check if the patient exists
            var patient = await _context.Patients.FindAsync(prescriptionDto.Patient.IdPatient);
            if (patient == null)
            {
                // Add new patient
                patient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    Birthdate = prescriptionDto.Patient.Birthdate
                };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            // Check if each medicament exists
            foreach (var medicamentDto in prescriptionDto.Medicaments)
            {
                var medicament = await _context.Medicaments.FindAsync(medicamentDto.IdMedicament);
                if (medicament == null)
                {
                    return BadRequest($"Medicament with Id {medicamentDto.IdMedicament} does not exist.");
                }
            }

            // Add the prescription
            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = prescriptionDto.Doctor.IdDoctor
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            // Add the PrescriptionMedicaments
            foreach (var medicamentDto in prescriptionDto.Medicaments)
            {
                var prescriptionMedicament = new PrescriptionMedicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = medicamentDto.IdMedicament,
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Details
                };
                _context.PrescriptionMedicaments.Add(prescriptionMedicament);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
