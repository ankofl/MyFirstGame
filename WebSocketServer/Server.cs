using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Text;

class Server
{
    private static async Task HandleClientAsync(HttpListenerContext context)
    {
        HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
        WebSocket webSocket = webSocketContext.WebSocket;

        Console.WriteLine("Client connected!");

        while (webSocket.State == WebSocketState.Open)
        {
            byte[] buffer = new byte[1024];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");

                // Отправка обратно клиенту
                byte[] responseBuffer = Encoding.UTF8.GetBytes($"Server received: {receivedMessage}");
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        Console.WriteLine("Client disconnected!");
    }

    static async Task Main()
    {
        string host = "http://194.87.95.150:1234/";

        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add(host);
        httpListener.Start();

        Console.WriteLine($"Server listening on {host}");

        while (true)
        {
            HttpListenerContext context = await httpListener.GetContextAsync();
            _ = HandleClientAsync(context);
        }
    }
}