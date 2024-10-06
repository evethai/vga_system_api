using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Student;
using Domain.Model.Wallet;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly VgaDbContext _context;
        public WalletRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<WalletModel>> GetInforStudentHasWalletReceiving(Guid id)
        {

            var idHighschool = _context.HighSchool
                .Where(a => a.AccountId.Equals(id))
                .FirstOrDefault(); 
            var listStudent = await _context.Student
                .Where(a => a.HighSchoolId.Equals(idHighschool.Id)).AsNoTracking()        
                .ToListAsync();            
            var wallets = await _context.Wallet
                .Where(w => listStudent.Select(s => s.AccountId).Contains(w.AccountId)).AsNoTracking()
                .ToListAsync();
            var responseWallets = wallets.Select(w => new WalletModel
            {
                Id = w.Id,
                AccountId = w.AccountId,
                GoldBalance = w.GoldBalance,               
            }).ToList();
            return responseWallets;
        }
    }
}
