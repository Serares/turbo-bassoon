using Microsoft.AspNetCore.SignalR.Client;
using Northwind.Chat.Models;

Write("Enter a username:");
string? username = ReadLine();

if (string.IsNullOrEmpty(username))
{
    WriteLine("You must enter ausername to register with chat!");
    return;
}

Write("Enter your groups (otpional):");

string? groups = ReadLine();

HubConnection hubConnection = new HubConnectionBuilder()
    .WithUrl("https://localhost:5111/chat")
    .Build();

hubConnection.On<MessageModel>("ReceiveMessage", message =>
{
    WriteLine($"To {message.To}, From {message.From}: {message.Body}");
});

await hubConnection.StartAsync();

UserModel registration = new()
{
    Name = username,
    Groups = groups
};


await hubConnection.InvokeAsync("Register", registration);

WriteLine("Successfully registed.");
WriteLine("Listening .... {press enter to stop}");
ReadLine();