using System.Collections.Generic;
namespace Cw6.Models.Dto;
public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}