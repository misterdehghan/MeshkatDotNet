using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Common.Pagination
{
    public static class Pagination
    {

        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int pageSize, out int rowsCount)
        {
            if (source != null)
            {
                rowsCount = source.Count();
            }
            else
            {
                rowsCount = 0;
            }

            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public static IQueryable<TSource> PagedResult<TSource>(this IQueryable<TSource> query, int pageNum, int pageSize, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;
            //مجموع ردیف‌های به دست آمده
            rowsCount = query.Count();
            // اگر شماره صفحه کوچکتر از 0 بود صفحه اول نشان داده شود
            if (pageNum <= 0) pageNum = 1;
            // محاسبه ردیف هایی که نسبت به سایز صفحه باید از آنها گذشت
            int excludedRows = (pageNum - 1) * pageSize;
            // ردشدن از ردیف‌های اضافی و  دریافت ردیف‌های مورد نظر برای صفحه مربوطه
            return query.Skip(excludedRows).Take(pageSize);
        }
    }
}
