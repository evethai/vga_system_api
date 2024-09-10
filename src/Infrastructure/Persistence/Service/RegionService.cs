using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Region;

namespace Infrastructure.Persistence.Service;
public class RegionService : IRegionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegionModel> GetAllRegion()
    {
        var regions = await _unitOfWork.RegionRepository.GetAllAsync();
        var result = _mapper.Map<List<RegionModel>>(regions);
        return new ResponseRegionModel
        {
            regions = result
        };
    }

    public async Task<RegionModel> GetRegionById(int id)
    {
        var region = await _unitOfWork.RegionRepository.GetByIdAsync(id);
        return _mapper.Map<RegionModel>(region);
    }
}
