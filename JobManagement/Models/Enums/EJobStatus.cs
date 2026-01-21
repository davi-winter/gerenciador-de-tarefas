using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JobManagement.Models.Enums
{
    public enum EJobStatus : int
    {
        [Display(Name = "Pendente")]
        Pending = 1,
        [Display(Name = "Em andamento")]
        InProgress = 2,
        [Display(Name = "Concluída")]
        Completed = 3
    }
}
