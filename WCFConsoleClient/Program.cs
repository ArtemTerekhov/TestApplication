using System;

namespace WCFConsoleClient
{
    internal class Program
    {
        static void Main()
        {
            Service1Client client = new Service1Client("BasicHttpBinding_IService1");
            Console.Write("Введите город для подсчета количества клиентов:");
            var location = Console.ReadLine();
            Console.WriteLine(client.GetData(location.ToString()));
            Console.WriteLine("Нажмите \"Enter\" для закрытия окна приложения");
            Console.ReadLine();
        }
    }
}
