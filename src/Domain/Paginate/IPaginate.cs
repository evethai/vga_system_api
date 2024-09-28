using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Paginate
{
    public interface IPaginate<T>
    {
        int Size { get; }
        int Page { get; }
        int Total { get; }
        int TotalPages { get; }
        IList<T> Items { get; }
    }
}
