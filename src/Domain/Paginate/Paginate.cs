using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Paginate
{
    public class Paginate<T> : IPaginate<T>
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Items { get; set; }

        public Paginate(IEnumerable<T> source, int page, int size, int firstPage)
        {
            var enumerable = source as T[] ?? source.ToArray();
            if (firstPage > page)
            {
                throw new ArgumentException($"Page ({page}) must be greater or equal than firstPage ({firstPage})");
            }

            if (source is IQueryable<T> queryable)
            {
                Page = page;
                Size = size;
                Total = queryable.Count();
                Items = queryable.Skip((page - firstPage) * size).Take(size).ToList();
                TotalPages = (int)Math.Ceiling(Total / (double)Size);
            }
            else
            {
                Page = page;
                Size = size;
                Total = enumerable.Length;
                Items = enumerable.Skip((page - firstPage) * size).Take(size).ToList();
                TotalPages = (int)Math.Ceiling(Total / (double)Size);
            }
        }

        public Paginate()
        {
            Items = Array.Empty<T>();
        }
    }
}
