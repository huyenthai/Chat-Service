using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.Tests
{
    public class FakeClientProxy: IClientProxy
    {
        public string? LastMethod { get; private set; }
        public object[]? LastArgs { get; private set; }
        public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
        {
            LastMethod = method;
            LastArgs = args;
            return Task.CompletedTask;
        }
    }
}
