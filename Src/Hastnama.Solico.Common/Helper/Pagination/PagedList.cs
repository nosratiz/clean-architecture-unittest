using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Common.Helper.Pagination
{
    public class PagedList<T>
    {
        public int CurrentPage { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public int TotalSize { get; }

        public List<T> Items { get; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalSize = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items.ToList();
        }

        public PagedList<TDest> MapTo<TDest>(IMapper mapper)
        {
            var items = mapper.Map<List<TDest>>(Items);
            return new PagedList<TDest>(items, TotalSize, CurrentPage, PageSize);
        }

        public static PagedList<T> Create(IQueryable<T> query, int pageNumber, int pageSize, int count)
        {
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize, int count, CancellationToken cancellationToken)
        {
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> CreateAsync(IEnumerable<T> query, int pageNumber, int pageSize, int count)
        {
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}