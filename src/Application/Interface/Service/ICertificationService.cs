using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Certification;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface ICertificationService
    {
        Task<ResponseModel> GetCertificationByIdAsync(int id);
        Task<ResponseModel> CreateCertificationAsync(CertificationPostModel postModel, Guid consultantId);
        Task<ResponseModel> DeleteCertificationAsync(int certificationId);
    }
}
