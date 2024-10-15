using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Admin;

namespace Application.Interface.Service
{
    public interface IAdminService
    {
        Task<DashboardModel> GetDashboard();
    }
}
