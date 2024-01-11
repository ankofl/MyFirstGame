using System.Net.Sockets;
using System.Net;
using System.Text;

public class SocketServer
{
    public static int fact(int x)//Вычисление факториала
    {
        int z = 1;
        if ((x == 0) || (x == 1))
        {
            return 1;          //Если х=0 или х=1 то возвращаем значение 1
        }
        else
        {
            for (int i = 2; i <= x; i++)
            {
                z = z * i;
            }
            return z;
        }

    }

    public static int MINARGUMENT(double[] a, int n) // Нахождения индекса с минимальным значением суммы времени работы системы и времени обслуживания заявки
    {
        double min = a[0];
        int indexmin = 0;
        for (int i = 1; i <= n; i++)
        {
            if (a[i - 1] < min)
            {
                min = a[i - 1];
                indexmin = i - 1;
            }
        }
        return indexmin;
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Write server's ip address");
        Console.ReadKey();


        string hostName = "localhost";
        IPAddress ipAddress = Dns.GetHostAddresses(hostName)[1];


        IPEndPoint ipendpoint = new IPEndPoint(ipAddress, 1234);//Подготовка адреса локальной конечной точки
        Socket slistener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Создание потокового сокета
        try //Инструкция для поиска исключительных ситуаций
        {
            slistener.Bind(ipendpoint);//Присвоение сокету имени и связываниее сокета с конечной локальной точкой
            slistener.Listen(100);//Начинаем слушать соединение и определение максимальное количество соединений
            while (true)
            {
                Console.WriteLine("Waiting... {0}", ipendpoint);
                Socket handler = slistener.Accept();//Программа приостанавливается, ожидая входящие соединения
                string data = null;//Создание строки с именем data и присвоение ей значение null
                while (true)
                {
                    byte[] bytes = new byte[1024];//Формирование массива байтов длиной 1024
                    int bytesrec = handler.Receive(bytes);//Заполнение массива bytes сообщением от клиента и присваивание byterec длины сообщения
                    data = Encoding.ASCII.GetString(bytes, 0, bytesrec);//присваивание массива bytes и указание его длины 
                    Console.WriteLine("Parameters: {0}", data);//Вывод полученных данных
                    string[] quest = data.Split(';');//Создание массива quest и присваивание ему строки разбитой на элементы
                    double l = Convert.ToDouble(quest[0]);//Создание переменной l и присваивание ей элемента массива quest с индексом 0 
                    double mu = Convert.ToDouble(quest[1]);//Создание переменной mu и присваивание ей элемента массива quest с индексом 1 
                    int n = Convert.ToInt32(quest[2]);//Создание переменной n и присваивание ей элемента массива quest с индексом 2

                    if (n == 0)//Если количество каналов равно 0, то выводим сообщение об ошибке
                    {
                        Console.WriteLine("Error!");
                    }
                    else
                    {
                        double a = l / mu;//Нахождение времени обслуживания одной заявки
                        double p0 = 0;
                        for (int i = 1; i <= n; i++)
                        {
                            p0 = p0 + (Math.Exp(i * Math.Log(a))) / fact(i);
                        }
                        p0 = p0 + 1 + (Math.Exp((n + 1) * Math.Log(a))) / (fact(n) * (n - a));
                        p0 = 1 / p0;//Вероятность того, что в узле нет очереди


                        double[] Pn = new double[10];//Создание массива Р длиной 50
                        string buff = "";//Создание строки buff 
                        for (int i = 0; i <= n; i++)
                        {
                            Pn[i] = p0 * ((a) / fact(i));//Вероятности того, что n каналов занято
                            buff += Pn[i].ToString() + ";";//Создание строки buff вида: P[1];P[2];P[3];...;P[n-1]
                        }

                        string mass = buff;//Создание переменной mass и присвоение ей значения строки buff
                        byte[] msgm = Encoding.ASCII.GetBytes(mass);//Создание массива байтов msgm и присвоение ему значение mass
                        handler.Send(msgm);//Отправка msgm клиенту

                        double[] Pnm = new double[10];
                        string buff1 = "";
                        for (int i = 0; i <= 4; i++)
                        {
                            Pnm[i] = p0 * (Math.Exp((n + i) * Math.Log(a))) / (fact(n) * Math.Exp(i * Math.Log(n)));//Вероятность того, что все каналы заняты и m заявок в очереди
                            buff1 += Pnm[i].ToString() + ";";
                        }

                        mass = buff1;
                        msgm = Encoding.ASCII.GetBytes(mass);
                        handler.Send(msgm);

                        double Poch = (Math.Exp((n + 1) * Math.Log(a))) / (fact(n) * (n - a));//Вероятность того, что в узле будет очередь
                        double Loch = (((Math.Exp((n + 1) * Math.Log(a))) * p0) / (n * fact(n))) * (1 / Math.Exp(2 * Math.Log(1 - a / n)));//Среднее число заявок в очереди на обслуживание
                        double Lsist = Loch + a;//Среднее число находящихся в системе заявок
                        double Toch = 1 / l * Loch;//Средняя продолжительность пребывания заявки в очереди
                        double Tsist = 1 / l * Lsist;//Средняя продолжительность пребывания заявки в системе
                        double[] MASSTIME = new double[n]; //Создание массива tp длиной n
                        int indexm = 0;
                        double tmin = 0;
                        double tsist = 0;
                        double t = 0;
                        double y = 0;
                        int Och = 0;
                        int Obsl = 0;
                        string themsg;
                        byte[] msg;
                        Random r = new Random();//Создание r и присвоение ей случайного  значения
                        while (true)
                        {
                            bytesrec = handler.Receive(bytes); //Заполнение массива bytes сообщением от клинта и присваивание в bytesrec длины сообщения
                            data = Encoding.ASCII.GetString(bytes, 0, bytesrec); //Присваивание массива bytes и указание его длины
                            if (data.IndexOf("fin") > -1) //Проверка на наличие а data слова fin и если оно найдено, то возвращаем значение 0   
                            {
                                break;
                            }
                            Obsl++; //Количесво поступивших заявок
                            t = Convert.ToDouble(data);
                            indexm = MINARGUMENT(MASSTIME, n);
                            tmin = MASSTIME[indexm];
                            if (tmin < t)
                            {
                                msg = Encoding.ASCII.GetBytes("true");
                                handler.Send(msg);
                                y = -Math.Log(r.NextDouble()) / mu; //Время обслуживание заявки по показательному закону(Пуассона)
                                tsist = tsist + y; //Время заявок в системе 
                                MASSTIME[indexm] = t + y; //Сумма времени системы и обслуживание заявки
                            }
                            else
                            {
                                msg = Encoding.ASCII.GetBytes("false");
                                handler.Send(msg);
                                Och++; //Количество заявок в очереди
                            }
                        }
                        tsist = tsist / Obsl;
                        double lsist = tsist * l;
                        double loch = lsist - a;
                        double toch = 1 / l * loch;


                        themsg = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", Convert.ToString(loch), Convert.ToString(lsist), Convert.ToString(toch), Convert.ToString(tsist), Convert.ToString(p0), Convert.ToString(Poch), Convert.ToString(Loch), Convert.ToString(Lsist), Convert.ToString(Toch), Convert.ToString(Tsist));
                        msg = Encoding.ASCII.GetBytes(themsg);
                        handler.Send(msg);
                        if (data.IndexOf("fin") > -1)
                        {
                            break;
                        }
                    }
                    handler.Shutdown(SocketShutdown.Both);//Останавливаем отправку и получение данных сокетом
                    handler.Close();//Закрытие сокета
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error");
        }
        Console.ReadKey();
    }
}

    