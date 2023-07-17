using Microsoft.AspNetCore.SignalR;

namespace PlanIt.Plan.Application.Hubs;

public class PlanHub : Hub
{
    //Client subscribes to events
    public async Task SubscribeToPlan(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        Console.WriteLine($"{userId} subscribed!");
    }
    
    //Client unsubscribes 
    //will almost never be used. ( only sign - out )
    public async Task UnsubscribeFromPlan(string userId, CancellationToken cancellationToken)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId, cancellationToken);
        Console.WriteLine($"{userId} unsubscribed!");
    }
}