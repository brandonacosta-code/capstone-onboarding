using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Capstone.Application.MediatR
{
    public interface IMessageBus
    {
        TResponse Send<TResponse>(IRequest<TResponse> request);
        Task SendAsync<TResponse>(IRequest<TResponse> request);
    }

    public class MessageBus : IMessageBus
    {
        private readonly IMediator _mediator;

        public MessageBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            return _mediator.Send(request).GetAwaiter().GetResult();
        }

        public async Task SendAsync<TResponse>(IRequest<TResponse> request)
        {
            await _mediator.Send(request).ConfigureAwait(false);
        }
    }

    public class Ping : IRequest<bool>
    {
        public string Name { get; set; }
    }

    public class PingHandler : IRequestHandler<Ping, bool>
    {

        public PingHandler()
        {
        }

        public Task<bool> Handle(Ping request, CancellationToken cancellationToken)
        {
            Console.WriteLine($@"This is ping {request.Name}");
            return Task.FromResult(true);
        }
    }
}
