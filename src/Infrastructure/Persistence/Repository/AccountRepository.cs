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

            var email = await _context.Account.Where(x => x.Email.Equals(registerAccount.Email)|| x.Phone.Equals(registerAccount.Phone)).FirstOrDefaultAsync();
            if (email != null)
                throw new Exception($"Email hoặc số điện thoại của '{registerAccount.Name}' đã tồn tại trong hệ thống");
            
            
            registerAccount.Phone = string.Concat("84", registerAccount.Phone.AsSpan(1));
            Account account = null;
            switch (_role)
            {
                case RoleEnum.Student:                  
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Name = registerAccount.Name,
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        Role = _role,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow.AddHours(7)
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.Consultant:
                    
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Name = registerAccount.Name,
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        Role = _role,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow.AddHours(7)
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.HighSchool:                    
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Name = registerAccount.Name,
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        Role = _role,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow.AddHours(7)
                    };
                    account.Wallet = new Wallet
                    {
                        Id = Guid.NewGuid(),
                        GoldBalance = 0,
                        AccountId = account.Id,
                    };
                    break;
                case RoleEnum.University:
                    account = new Account
                    {
                        Id = Guid.NewGuid(),
                        Name = registerAccount.Name,
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        Password = PasswordUtil.HashPassword(registerAccount.Password),
                        Role = _role,
                        Status = AccountStatus.Active,
                        CreateAt = DateTime.UtcNow.AddHours(7)
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
