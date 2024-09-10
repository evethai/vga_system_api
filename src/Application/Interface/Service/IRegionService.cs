using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Region;

namespace Application.Interface.Service;
public interface IRegionService
{
    Task<ResponseRegionModel> GetAllRegion();
    Task<RegionModel> GetRegionById(int id);
}
