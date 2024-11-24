using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Student;
using Domain.Model.Transaction;
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
    }
}
