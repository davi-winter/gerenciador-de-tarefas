using JobManagement.Models;
using JobManagement.Models.Enums;

namespace JobManagement.Data
{
    public class SeedingService
    {
        private JobManagementDbContext _context;

        public SeedingService(JobManagementDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.User.Any() ||
                _context.Job.Any())
            {
                return;
            }

            User u1 = new User(1, "José", "57572785018", "jose@gmail.com", "5438250982", new DateTime(1980, 1, 5));
            User u2 = new User(2, "Maria", "47956029058", "maria@gmail.com", "53982574212", new DateTime(1997, 12, 10));
            User u3 = new User(3, "João", "27022207038", "joao@gmail.com", "5325860194", new DateTime(2000, 3, 27));
            User u4 = new User(4, "Fernanda", "00777648032", "fernanda@gmail.com", "51997321287", new DateTime(1988, 6, 15));

            Job j1 = new Job(1, "Tarefa 1", EJobStatus.InProgress, 70, DateTime.Now.AddDays(-25), EJobPriority.Medium, u1);
            Job j2 = new Job(2, "Tarefa 2", EJobStatus.Completed, 100, DateTime.Now.AddDays(-21), EJobPriority.Minimum, u2);
            Job j3 = new Job(3, "Tarefa 3", EJobStatus.InProgress, 50, DateTime.Now.AddDays(-17), EJobPriority.Medium, u3);
            Job j4 = new Job(4, "Tarefa 4", EJobStatus.InProgress, 35, DateTime.Now.AddDays(-10), EJobPriority.Low, u4);
            Job j5 = new Job(5, "Tarefa 5", EJobStatus.Pending, 0, DateTime.Now.AddDays(2), EJobPriority.Top, u1);
            Job j6 = new Job(6, "Tarefa 6", EJobStatus.Completed, 100, DateTime.Now.AddDays(4), EJobPriority.High, u2);
            Job j7 = new Job(7, "Tarefa 7", EJobStatus.InProgress, 80, DateTime.Now.AddDays(8), EJobPriority.Low, u3);
            Job j8 = new Job(8, "Tarefa 8", EJobStatus.Completed, 100, DateTime.Now.AddDays(14), EJobPriority.Low, u4);
            Job j9 = new Job(9, "Tarefa 9", EJobStatus.Pending, 0, DateTime.Now.AddDays(35), EJobPriority.Medium, u1);
            Job j10 = new Job(10, "Tarefa 10", EJobStatus.InProgress, 20, DateTime.Now.AddDays(40), EJobPriority.Minimum, u2);
            Job j11 = new Job(11, "Tarefa 11", EJobStatus.Pending, 0, DateTime.Now.AddDays(47), EJobPriority.Medium, u3);
            Job j12 = new Job(12, "Tarefa 12", EJobStatus.Completed, 100, DateTime.Now.AddDays(55), EJobPriority.High, u4);

            _context.User.AddRange(u1, u2, u3, u4);

            _context.Job.AddRange(j1, j2, j3, j4, j5, j6, j7, j8, j9, j10, j11, j12);

            _context.SaveChanges();
        }
    }
}