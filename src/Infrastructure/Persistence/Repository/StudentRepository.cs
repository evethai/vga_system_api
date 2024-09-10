using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Student;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository;
public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(VgaDbContext context) : base(context)
    {
    }

    public (Expression<Func<Student, bool>> filter, Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy) BuildFilterAndOrderBy(StudentSearchModel searchModel)
    {
        Expression<Func<Student, bool>> filter = p => true;
        Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy = null;
        if (!string.IsNullOrEmpty(searchModel.name))
        {
            filter = filter.And(p => p.Name.Contains(searchModel.name));
        }

        if (searchModel.highschoolId.HasValue)
        {
            filter = filter.And(p => p.HighSchoolId == searchModel.highschoolId.Value);
        }

        if (searchModel.sortByGold == true)
        {
            orderBy = p => searchModel.descending == true ?
                           p.OrderByDescending(x => x.GoldBalance) :
                           p.OrderBy(x => x.GoldBalance);
        }
        return (filter, orderBy);
    }
}
