using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Highschool;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository;
public class HighschoolRepository : GenericRepository<HighSchool>, IHighschoolRepository
{
    public HighschoolRepository(VgaDbContext context) : base(context)
    {
    }

    public (Expression<Func<HighSchool, bool>> filter, Func<IQueryable<HighSchool>, IOrderedQueryable<HighSchool>> orderBy) BuildFilterAndOrderBy(HighschoolSearchModel searchModel)
    {
        Expression<Func<HighSchool, bool>> filter = p => true;
        Func<IQueryable<HighSchool>, IOrderedQueryable<HighSchool>> orderBy = null;
        if (!string.IsNullOrEmpty(searchModel.name))
        {
            filter = filter.And(p => p.Name.Contains(searchModel.name));
        }

        if (searchModel.regionId.HasValue)
        {
            filter = filter.And(p => p.RegionId == searchModel.regionId.Value);
        }

        if (searchModel.sortByGold == true)
        {
            //orderBy = p => searchModel.descending == true ?
            //               p.OrderByDescending(x => x.GoldBalance) :
            //               p.OrderBy(x => x.GoldBalance);
        }
        return (filter, orderBy);
    }
}
