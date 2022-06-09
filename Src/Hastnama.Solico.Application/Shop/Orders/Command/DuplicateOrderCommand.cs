using System;
using System.Drawing;
using AutoMapper;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Orders.Command
{
    public class DuplicateOrderCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}