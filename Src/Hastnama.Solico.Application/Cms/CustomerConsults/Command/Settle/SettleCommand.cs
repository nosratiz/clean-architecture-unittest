using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Command.Settle
{
    public class SettleCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}