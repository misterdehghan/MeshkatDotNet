using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Azmoon.Application.Service.Login.Command
{
    public interface ILoginCRUD
        {
        Task<IEnumerable<LoginLog>> GetAllAsync();
        Task<IPagedList<LoginLog>> GetPagedListAsync(int pageNumber, int pageSize);
        Task<LoginLog> GetByIdAsync(long id);
        Task CreateAsync(LoginLog log);
        Task UpdateAsync(LoginLog log);
        Task DeleteAsync(long id);
        }

    public class LoginCRUD : ILoginCRUD
        {
        private readonly IDataBaseContext _context;

        public LoginCRUD(IDataBaseContext context)
            {
            _context = context;
            }

        public async Task<IEnumerable<LoginLog>> GetAllAsync()
            {
            return await _context.LoginLogs.ToListAsync();
            }

        public async Task<IPagedList<LoginLog>> GetPagedListAsync(int pageNumber, int pageSize)
            {
            var query = _context.LoginLogs.OrderByDescending(l => l.RegesterAt);
            return await query.ToPagedListAsync(pageNumber, pageSize);
            }

        public async Task<LoginLog> GetByIdAsync(long id)
            {
            return await _context.LoginLogs.FindAsync(id);
            }

        public async Task CreateAsync(LoginLog log)
            {
            await _context.LoginLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            }

        public async Task UpdateAsync(LoginLog log)
            {
            _context.LoginLogs.Update(log);
            await _context.SaveChangesAsync();
            }

        public async Task DeleteAsync(long id)
            {
            var log = await _context.LoginLogs.FindAsync(id);
            if (log != null)
                {
                _context.LoginLogs.Remove(log);
                await _context.SaveChangesAsync();
                }
            }
        }

    }
