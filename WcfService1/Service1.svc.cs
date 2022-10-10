using System.Linq;
using WcfService1.Models;

namespace WcfService1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService1
    {
        readonly ApplicationContext db = new ApplicationContext();
        public string GetData(string value)
        {
            var clientsByTown = from clients in db.Clients
                                join locations in db.Locations on clients.LocationId
                                equals locations.LocationId
                                where locations.Name == value
                                select clients.ClientId;

            return clientsByTown.Count().ToString();
        }

    }
}
