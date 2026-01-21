using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using JobManagement.Models.Enums;

namespace JobManagement.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} requerida")]
        [Display(Name = "Descrição")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve conter entre {2} e {1} caracteres")]
        public string Description { get; set; }

        public EJobStatus Status { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [Range(0, 100, ErrorMessage = "O valor deve ser entre {1} e {2}.")]
        [Display(Name = "Progresso")]
        public int Progress { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [Display(Name = "Prazo")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "Prioridade")]
        public EJobPriority Priority { get; set; }

        [Display(Name = "Usuário")]
        public User User { get; set; }

        [Display(Name = "Usuário")]
        public int UserId { get; set; }

        public Job()
        {
        }

        public Job(int id, string description, EJobStatus status, int progress, DateTime deadline, EJobPriority priority, User user)
        {
            Id = id;
            Description = description;
            Status = status;
            Progress = progress;
            Deadline = deadline;
            Priority = priority;
            User = user;
        }
    }
}
