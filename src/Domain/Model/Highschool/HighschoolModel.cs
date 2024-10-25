using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.AccountWallet;

namespace Domain.Model.Highschool;
public class HighschoolModel
{
    public Guid Id { get; set; } 
    public string Address { get; set; }
    public Guid RegionId { get; set; }
    public AccountWalletModel Account { get; set; }

}
public class ResponseHighSchoolModel
{
    public int? total { get; set; }
    public int? currentPage { get; set; }
    public List<HighschoolModel> highschools { get; set; }
}
