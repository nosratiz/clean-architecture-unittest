using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Excel;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Activation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Create;
using Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class CustomersController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCustomerPagedListQuery model,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(model, cancellationToken));


        [HttpGet("ExcelReport")]
        public async Task<IActionResult> GetExcelReport(CancellationToken cancellationToken)
        {
            var customers = await _mediator.Send(new GetCustomerListQuery(), cancellationToken);

            var url = ReportGenerator.CustomerList(customers);

            return Ok(new ReportDto { Url = url });
        }


        [HttpPut("SetPassword")]
        public async Task<IActionResult> UpdatePassword(UpdateCustomerCommand updateCustomerCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateCustomerCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpGet("{solicoCustomerId}", Name = "GetInfo")]
        public async Task<IActionResult> GetInfo(string solicoCustomerId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCustomerQuery { SolicoCustomerId = solicoCustomerId },
                cancellationToken);

            return result.ApiResult;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand createCustomerCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCustomerCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return CreatedAtAction(nameof(GetInfo), new { solicoCustomerId = result.Data.SolicoCustomerId },
                result.Data);
        }


        [HttpGet("customerEnrollment")]
        public async Task<IActionResult> GetCustomerEnrollment(
            [FromQuery] GetCustomerEnrollmentPagedListQuery getCustomerEnrollmentPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getCustomerEnrollmentPagedListQuery, cancellationToken));


        [HttpPut("{id}/activation")]
        public async Task<IActionResult> Activation(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ActivationCustomerCommand { Id = id }, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}