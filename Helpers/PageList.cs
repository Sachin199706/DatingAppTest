using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DatingApp.Helpers
{
    public class PageList<T>:List<T>
    {
        public int Totalcount { get; set; }
        public int PageSizes { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage{ get; set; }

        public PageList(IEnumerable<T> items,int count,int pageNumber,int pageSize) {
            Totalcount = count;
            PageSizes = pageSize;
            CurrentPage = pageNumber;
            TotalPage =(int) Math.Ceiling((count / (double)pageSize));
            AddRange(items);

        }
        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source,int pageNumber,int pageSize)
        {
            var count=await source.CountAsync();
            var item=await source.Skip((pageNumber-1)* pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(item, count, pageNumber, pageSize);
        }

        
    }
}
