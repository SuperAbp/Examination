using System;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client;

public class NotificationStateService
{
    private event Func<Task>? Changed;

    public void Subscribe(Func<Task> handler)
    {
        Changed += handler;
    }

    public void Unsubscribe(Func<Task> handler)
    {
        Changed -= handler;
    }

    public async Task NotifyChangedAsync()
    {
        if (Changed == null)
        {
            return;
        }

        foreach (Func<Task> handler in Changed.GetInvocationList())
        {
            await handler();
        }
    }
}

