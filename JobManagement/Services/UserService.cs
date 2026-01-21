using JobManagement.Data;
using JobManagement.Models;
using JobManagement.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobManagement.Services
{
    public class UserService
    {
        private readonly JobManagementDbContext _context;

        public UserService(JobManagementDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> FindAllAsync(string fieldOrder)
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

            return await _context.User.AsQueryable().OrderBy(orderByString).ToListAsync();
        }

        public async Task InsertAsync(User obj)
        {
            // Remove as pontuações de máscaras para o CPF e Telefone
            if (!string.IsNullOrEmpty(obj.Cpf))
                obj.Cpf = System.Text.RegularExpressions.Regex.Replace(obj.Cpf, @"[^\d]", "");
            if (!string.IsNullOrEmpty(obj.Telephone))
                obj.Telephone = System.Text.RegularExpressions.Regex.Replace(obj.Telephone, @"[^\d]", "");

            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.User.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.User.FindAsync(id);
            _context.User.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User obj)
        {
            // Remove as pontuações de máscaras para o CPF e Telefone
            if (!string.IsNullOrEmpty(obj.Cpf))
                obj.Cpf = System.Text.RegularExpressions.Regex.Replace(obj.Cpf, @"[^\d]", "");
            if (!string.IsNullOrEmpty(obj.Telephone))
                obj.Telephone = System.Text.RegularExpressions.Regex.Replace(obj.Telephone, @"[^\d]", "");

            bool hasAny = await _context.User.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrado");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
