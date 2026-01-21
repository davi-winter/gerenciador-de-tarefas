using System.ComponentModel.DataAnnotations;

namespace JobManagement.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [Display(Name = "Nome")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "{0} deve conter 11 dígitos")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        public string FormattedCpf => FormatCpf(Cpf);

        [EmailAddress(ErrorMessage = "{0} deve ser válido")]
        public string? Email { get; set; }

        [StringLength(15, MinimumLength = 14, ErrorMessage = "{0} deve conter entre 10 e 11 dígitos")]
        [Display(Name = "Telefone")]
        public string? Telephone { get; set; }
        public string FormattedPhone => FormatPhone(Telephone);

        [Required(ErrorMessage = "{0} requerida")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public User()
        {
        }

        public User(int id, string name, string cpf, string email, string telephone, DateTime birthDate)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
            Telephone = telephone;
            BirthDate = birthDate;
        }

        public void AddJob(Job job)
        {
            Jobs.Add(job);
        }

        public void RemoveJob(Job job)
        {
            Jobs.Remove(job);
        }

        private string FormatPhone(string? number)
        {
            if (string.IsNullOrEmpty(number)) 
                return string.Empty;

            // Remove caracteres não numéricos
            var justDigits = new string(number.Where(char.IsDigit).ToArray());

            switch (justDigits.Length)
            {
                case 10: // Fixo ou celular antigo: (XX) XXXX-XXXX
                    return Convert.ToUInt64(justDigits).ToString(@"(00) 0000-0000");
                case 11: // Celular com 9 dígitos: (XX) 9XXXX-XXXX
                    return Convert.ToUInt64(justDigits).ToString(@"(00) 00000-0000");
                default:
                    return justDigits; // Retorna sem formatação se o tamanho for inválido
            }
        }

        private string FormatCpf(string number)
        {
            if (string.IsNullOrEmpty(number))
                return string.Empty;

            var justDigits = new string(number.Where(char.IsDigit).ToArray());

            switch (justDigits.Length)
            {
                case 11: // Máscara do CPF
                    return Convert.ToInt64(justDigits).ToString(@"000\.000\.000-00");
                default:
                    return justDigits;
            }
        }
    }
}
