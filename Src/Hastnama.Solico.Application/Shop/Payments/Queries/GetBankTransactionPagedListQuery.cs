using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Payments.Dto;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Shop.Payments.Queries
{
    public class GetBankTransactionPagedListQuery : PagingOptions,IRequest<PagedList<BankTransactionDto>>
    {
        
    }
    
    public class GetBankTransactionPagedListQueryHandler : PagingService<BankTransaction>, IRequestHandler<GetBankTransactionPagedListQuery,PagedList<BankTransactionDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetBankTransactionPagedListQueryHandler(ISolicoDbContext context, IMapper mapper, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<PagedList<BankTransactionDto>> Handle(GetBankTransactionPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BankTransaction> bankTransactions =  _context.BankTransactions
                .OrderByDescending(x=>x.CreateDate)
                .Include(x => x.CustomerOpenItem)
                .Include(x => x.Order).Where(x =>
                    x.Order.SolicoCustomerId == _currentCustomerService.CustomerId ||
                    x.CustomerOpenItem.CustomerId == Guid.Parse(_currentCustomerService.Id));


            var bankTransactionList =
                await GetPagedAsync(request.Page, request.Limit, bankTransactions, cancellationToken);

            return bankTransactionList.MapTo<BankTransactionDto>(_mapper);
        }
    }
}