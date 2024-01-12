using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Text;

class Client
{
    static async Task Main()
    {
        using (ClientWebSocket clientWebSocket = new ClientWebSocket())
        {
            Uri serverUri = new Uri("ws://194.87.95.150:1234");
            await clientWebSocket.ConnectAsync(serverUri, CancellationToken.None);

            Console.WriteLine("Connected to the server!");

            await SendAsync(clientWebSocket, "Hello from the client!");
            await ReceiveAsync(clientWebSocket);
        }
    }

    static async Task SendAsync(ClientWebSocket clientWebSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    static async Task ReceiveAsync(ClientWebSocket clientWebSocket)
    {
        byte[] buffer = new byte[1024];
        while (clientWebSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");
            }
        }
    }
}