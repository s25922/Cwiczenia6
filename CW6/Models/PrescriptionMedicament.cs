using System.ComponentModel.DataAnnotations.Schema;

namespace Cw6.Models;
using System.ComponentModel.DataAnnotations;
public class PrescriptionMedicament
{
    [Key]
    public int IdMedicament { get; set; }

    [Key]
    public int IdPrescription { get; set; }

    public int Dose { get; set; }

    [MaxLength(100)]
    public string Details { get; set; }

    [ForeignKey("IdMedicament")]
    public Medicament Medicament { get; set; }

    [ForeignKey("IdPrescription")]
    public Prescription Prescription { get; set; }
}
