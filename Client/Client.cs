using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
Console.ReadKey();
string host = "194.87.95.150";
string username = "root";
string password = "87jI2j6zac";
string command = "help";

try
{
    var processInfo = new ProcessStartInfo
    {
        FileName = "ssh",
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        Arguments = $"{username}@{host} {command}"
    };

    var process = new Process { StartInfo = processInfo };
    process.Start();

    // Передаем пароль на стандартный вход процесса (stdin)
    process.StandardInput.WriteLine(password);
    process.StandardInput.Close();

    // Получаем результат выполнения команды
    string result = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    process.WaitForExit();

    Console.WriteLine($"Результат выполнения команды 'ls':\n{result}");

    if (!string.IsNullOrWhiteSpace(error))
    {
        Console.WriteLine($"Ошибка:\n{error}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Произошла ошибка: {ex.Message}");
}

return;

try//Инструкция для поиска исключительных ситуаций
{
    byte[] bytes = new byte[1024];

    string hostName = "localhost";
    IPAddress ipaddr = Dns.GetHostAddresses(hostName)[1];

    IPEndPoint ipendpoint = new IPEndPoint(ipaddr, 1234);//Подготовка адреса локальной конечной точки
    Socket ssend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Создание потокового сокета
    ssend.Connect(ipendpoint);//Соединение сокета с удаленной конечной точкой

    byte[] msg = Encoding.ASCII.GetBytes("test");
    ssend.Send(msg);
    int bytesrec = ssend.Receive(bytes);
    string ansv = Encoding.ASCII.GetString(bytes, 0, bytesrec);
    string[] wansv = ansv.Split(';');

    int zayav = 1500;//Количество заявок, которые необходимо обслужить
    int Och = 0;
    double t = 0;
    string f = null;
    string date = null;
    Random r = new Random();
    for (int i = 0; i <= zayav; i++)
    {
        date = Convert.ToString(t);
        msg = Encoding.ASCII.GetBytes(date);
        ssend.Send(msg);
        bytesrec = ssend.Receive(bytes);
        f = Encoding.ASCII.GetString(bytes, 0, bytesrec);
        if (f == "false")
        {
            Och++; //Количество заявок в очереди
        }
    }

    msg = Encoding.ASCII.GetBytes("fin");
    ssend.Send(msg);
    bytesrec = ssend.Receive(bytes);
    ansv = Encoding.ASCII.GetString(bytes, 0, bytesrec);
    wansv = ansv.Split(';');
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
