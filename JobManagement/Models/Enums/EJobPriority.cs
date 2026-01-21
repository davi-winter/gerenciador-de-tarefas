using System.ComponentModel.DataAnnotations;

namespace JobManagement.Models.Enums
{
    public enum EJobPriority : int
    {
        [Display(Name = "Mínima")]
        Minimum = 1,
        [Display(Name = "Baixa")]
        Low = 2,
        [Display(Name = "Média")]
        Medium = 3,
        [Display(Name = "Alta")]
        High = 4,
        [Display(Name = "Máxima")]
        Top = 5
    }
}
