using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Student;
using Domain.Model.Wallet;

namespace Application.Interface.Repository;
public interface IStudentRepository: IGenericRepository<Student>
{
    (Expression<Func<Student, bool>> filter, Func<IQueryable<Student>, IOrderedQueryable<Student>> orderBy) BuildFilterAndOrderBy(StudentSearchModel searchModel);
}
