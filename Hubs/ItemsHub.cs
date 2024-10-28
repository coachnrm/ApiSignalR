using Microsoft.AspNetCore.SignalR;
using CrudSignalR.Models;

namespace CrudSignalR.Hubs 
{
    public class ItemsHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task ItemAdded(Item item)
    {
        await Clients.All.SendAsync("ItemAdded", item);
    }

     public async Task ItemUpdated(Item item)
    {
        await Clients.All.SendAsync("ItemUpdated", item);
    }

    public async Task ItemDeleted(int itemId)
    {
        await Clients.All.SendAsync("ItemDeleted", itemId);
    }
}
}