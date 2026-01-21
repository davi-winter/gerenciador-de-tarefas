namespace JobManagement.Models.ViewModels
{
    public class JobFormViewModel
    {
        public Job Job { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
