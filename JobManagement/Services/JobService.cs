using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using JobManagement.Data;
using JobManagement.Models;
using JobManagement.Models.Enums;
using JobManagement.Services.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobManagement.Services
{
    public class JobService
    {
        private readonly JobManagementDbContext _context;

        public static Expression<Func<Job, bool>> filterUser = x => 1 == 1;
        public static Expression<Func<Job, bool>> filterStatus = x => 1 == 1;
        public static Expression<Func<Job, bool>> filterPriority = x => 1 == 1;
        public static Expression<Func<Job, bool>> filterDates = x => 1 == 1;

        public JobService(JobManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> FindAllAsync(string fieldOrder)
        {
            string field = fieldOrder.Replace("_desc", "");
            string order;
            if (!fieldOrder.Contains("_desc"))
                order = "asc";
            else
                order = "desc";
            // Id é a ordenação padrão após a filtragem
            if (field == "Id")
                order = "asc";
            var orderByString = $"{field} {order}";

            return await _context.Job.Include(x => x.User).AsQueryable().OrderBy(orderByString).ToListAsync();
        }

        public async Task<List<Job>> FilterAsync(string fieldOrder, DateTime? minDate, DateTime? maxDate, int userId = 0,
            EJobStatus status = 0, EJobPriority priority = 0)
        {
            filterUser = x => 1 == 1;
            if (userId != 0)
                filterUser = x => x.UserId == userId;
            filterStatus = x => 1 == 1;
            if (status != 0)
                filterStatus = x => x.Status == status;
            filterPriority = x => 1 == 1;
            if (priority != 0)
                filterPriority = x => x.Priority == priority;
            filterDates = x => 1 == 1;
            if (minDate.HasValue && !maxDate.HasValue)
                filterDates = x => x.Deadline >= minDate;
            else if (!minDate.HasValue && maxDate.HasValue)
                filterDates = x => x.Deadline <= maxDate;
            else if (minDate.HasValue && maxDate.HasValue)
                filterDates = x => x.Deadline >= minDate && x.Deadline <= maxDate;

            string field = fieldOrder.Replace("_desc", "");
            string order;
            if (!fieldOrder.Contains("_desc"))
                order = "asc";
            else
                order = "desc";
            // Id é a ordenação padrão após a filtragem
            if (field == "Id")
                order = "asc";
            var orderByString = $"{field} {order}";

            return await _context.Job.Include(x => x.User)
                .Where(filterUser)
                .Where(filterStatus)
                .Where(filterPriority)
                .Where(filterDates)
                .AsQueryable().OrderBy(orderByString).ToListAsync();
        }

        public async Task InsertAsync(Job obj)
        {
            _context.Job.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Job> FindByIdAsync(int id)
        {
            return await _context.Job.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Job.FindAsync(id);
            _context.Job.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job obj)
        {
            Job objPrevious = obj;

            bool hasAny = await _context.Job.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new FileNotFoundException("Id não encontrado");
            }
            try
            {
                _context.Job.Update(obj);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
