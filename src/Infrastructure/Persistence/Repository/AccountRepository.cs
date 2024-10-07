using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Domain.Model.Response;
using Domain.Model.Wallet;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly VgaDbContext _context;
        public AccountRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Guid> CreateAccountAndWallet(RegisterAccountModel registerAccount, RoleEnum _role)
        {
            if (registerAccount == null)
            {
                throw new KeyNotFoundException("Null data");
            }
            Role role = null;
            Account account = null;
            switch (_role)
            {
                case RoleEnum.Student:
                    role = _context.Role.Where(a => a.Name.Equals(RoleEnum.Student.ToString())).FirstOrDefault();
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        RoleId = role.Id,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.Consultant:
                    role = _context.Role.Where(a => a.Name.Equals(RoleEnum.Consultant.ToString())).FirstOrDefault();
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        RoleId = role.Id,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.HighSchool:
                    role = _context.Role.Where(a => a.Name.Equals(RoleEnum.HighSchool.ToString())).FirstOrDefault();
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        RoleId = role.Id,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.University:
                    role = _context.Role.Where(a => a.Name.Equals(RoleEnum.University.ToString())).FirstOrDefault();
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        RoleId = role.Id,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                default:                   
                    break;
            }
            var result = await _context.Account.AddAsync(account);
            _context.SaveChanges();
            return account.Id;
        }
    }
}
