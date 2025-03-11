using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR; // to use Hub
using Northwind.Chat.Models;
namespace Northwind.SignalR.Service.Hubs;
public class ChatHub : Hub
{
    // a new instance of ChatHub is created to process each method
    // must store user names, connection IDs, and groups in a static field
    private static Dictionary<string, UserModel> Users = new();
    public async Task Register(UserModel newUser)
    {
        UserModel user;
        string action = "registered as a new user";

        // try to get a stored user with a match on new user.
        if (Users.ContainsKey(newUser.Name))
        {
            user = Users[newUser.Name];
            // remove any existing group registrations, Why ❓
            if (user.Groups is not null)
            {
                foreach (string group in user.Groups.Split(","))
                {
                    await Groups.RemoveFromGroupAsync(user.ConnectionId, group);
                }
            }

            user.Groups = newUser.Groups;

            // Connection Id might have changed if the browser refreshed so update it
            user.ConnectionId = Context.ConnectionId;
            action = "updated your registered user";
        }
        else
        {
            if (string.IsNullOrEmpty(newUser.Name))
            {
                // assign guid if anon
                newUser.Name = Guid.NewGuid().ToString();
            }

            newUser.ConnectionId = Context.ConnectionId;
            Users.Add(key: newUser.Name, value: newUser);
            user = newUser;
        }

        if (user.Groups is not null)
        {
            // a user does not have to belong to any groups
            // but if they do, register them with the Hub.
            foreach (string group in user.Groups.Split(","))
            {
                await Groups.AddToGroupAsync(user.ConnectionId, group);
            }
        }

        // send a message of success

        MessageModel message = new()
        {
            From = "SignalR Hub",
            To = user.Name,
            Body = string.Format(
                $"You have successfully {action} with connection ID {user.ConnectionId}"
            )
        };

        IClientProxy proxy = Clients.Client(user.ConnectionId);
        await proxy.SendAsync("ReceiveMessage", message);
    }

    public async Task SendMessage(MessageModel message)
    {
        IClientProxy proxy;

        if (string.IsNullOrEmpty(message.To))
        {
            message.To = "Everyone";
            proxy = Clients.All;
            await proxy.SendAsync("ReceiveMessage", message);
            return;
        }

        // split To into a list of user and group names.

        string[] userAndGroupList = message.To.Split(',');

        foreach (string userOrGroup in userAndGroupList)
        {
            if (Users.ContainsKey(userOrGroup))
            {
                // if the item is a user
                // send message to user
                // by looking for their connection id in the dict
                message.To = $"User: {Users[userOrGroup].Name}";
                proxy = Clients.Client(Users[userOrGroup].ConnectionId);
            }
            else
            {
                message.To = $"Group: {userOrGroup}";
                proxy = Clients.Group(userOrGroup);
            }

            await proxy.SendAsync("ReceiveMessage", message);
        }
    }
}