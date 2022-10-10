using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Tags("Методы работы с клиентами")]
    public class ClientsController : ControllerBase
    {
        readonly ApplicationContext db;
        public ClientsController(ApplicationContext context)
        {
            db = context;
        }

        /// <summary>
        /// Получить список клиентов. 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/getclients
        ///     
        /// </remarks>
        /// <response code="200">Возвращает список клиентов в формате JSON</response>
        /// <response code="400">Возвращает сообщение об ошибке запроса данных </response>
        [HttpGet]
        [Route("/api/getclients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await db.Clients.Select(c =>
                new { c.ClientId, c.Name, c.GenderId, c.LocationId })
                .ToListAsync();

            if (clients is not null)
                return Ok(clients);

            return BadRequest(new { errorText = "Ошибка в запросе данных" });
        }

        /// <summary>
        /// Получить информацию о клиенте. 
        /// </summary>
        /// <param name="clientId" example="1">
        /// Client ID
        /// </param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/getclient/1
        ///     
        /// </remarks>
        /// <response code="200">Возвращает информацию о клиенте в формате JSON</response>
        /// <response code="400">Возвращает сообщение об ошибке запроса данных </response>
        [HttpGet]
        [Route("/api/getclient/{clientId}")]
        public async Task<IActionResult> GetClient([Required] string clientId)
        {
            if (int.TryParse(clientId, out int id))
            {

                var client = await (from clients in db.Clients
                                    join genders in db.Genders on clients.GenderId
                                    equals genders.GenderId
                                    join locations in db.Locations on clients.LocationId
                                    equals locations.LocationId
                                    where clients.ClientId == id
                                    select new
                                    {
                                        clients.Name,
                                        Gender = genders.Name,
                                        Location = locations.Name
                                    }).FirstOrDefaultAsync();

                if (client is not null)
                    return Ok(client);

                return BadRequest(new { errorText = "Ошибка в запросе данных" });
            }

            return BadRequest(new { errorText = "Ошибка в запросе данных" });
        }

        /// <summary>
        /// Получить список полов. 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/getgenders
        ///     
        /// </remarks>
        /// <response code="200">Возвращает список полов в формате JSON</response>
        /// <response code="400">Возвращает сообщение об ошибке запроса данных </response>
        [HttpGet]
        [Route("/api/getgenders")]
        public async Task<IActionResult> GetGenders()
        {
            var genders = await db.Genders.Select(g =>
                new { g.GenderId, g.Name })
                .ToListAsync();

            if (genders is not null)
                return Ok(genders);

            return BadRequest(new { errorText = "Ошибка в запросе данных" });
        }

        /// <summary>
        /// Получить список населённых пунктов. 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /api/getlocations
        ///     
        /// </remarks>
        /// <response code="200">Возвращает список населённых пунктов в формате JSON</response>
        /// <response code="400">Возвращает сообщение об ошибке запроса данных </response>
        [HttpGet]
        [Route("/api/getlocations")]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await db.Locations.Select(l =>
                new { l.LocationId, l.Name })
                .ToListAsync();

            if (locations is not null)
                return Ok(locations);

            return BadRequest(new { errorText = "Ошибка в запросе данных" });
        }

        /// <summary>
        /// Добавление клиента. 
        /// </summary>
        /// <param name="inputs">Модель клиента в формате JSON</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /api/addclient
        ///     
        ///     тело запроса
        ///     {
        ///         "ClientId": 0,
        ///         "Name": "Артём Терехов",
        ///         "GenderId": 1,
        ///         "LocationId": 2
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Возвращает успешный результат добавления клиента</response>
        /// <response code="400">Возвращает сообщение об ошибке обработки данных</response>
        [HttpPost]
        [Route("/api/addclient")]
        public async Task<IActionResult> AddClient([FromBody][Required] ClientInputs inputs)
        {
            if (ModelState.IsValid)
            {
                Client client = await db.Clients.FirstOrDefaultAsync(c => c.Name == inputs.Name);

                if (client == null)
                {
                    db.Clients.Add(new Client
                    {
                        Name = inputs.Name,
                        GenderId = inputs.GenderId,
                        LocationId = inputs.LocationId
                    });

                    await db.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest(new { errorText = "Такой клиент уже существует" });
            }

            return BadRequest(new { errorText = "Ошибка в обработке данных {0}", 
                ModelState.Values });
        }

        /// <summary>
        /// Редактирование клиента. 
        /// </summary>
        /// <param name="inputs">Модель клиента в формате JSON</param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /api/editclient
        ///     
        ///     тело запроса
        ///     {
        ///         "ClientId": 1,
        ///         "Name": "Артём Терехов",
        ///         "GenderId": 1,
        ///         "LocationId": 2
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Возвращает успешный результат редактирования клиента</response>
        /// <response code="400">Возвращает сообщение об ошибке обработки данных</response>
        [HttpPut]
        [Route("/api/editclient")]
        public async Task<IActionResult> EditClient([FromBody][Required] ClientInputs inputs)
        {
            if (ModelState.IsValid && inputs.ClientId != 0)
            {
                Client client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId
                    == inputs.ClientId);

                if (client is not null)
                {
                    client.Name = inputs.Name;
                    client.GenderId = inputs.GenderId;
                    client.LocationId = inputs.LocationId;

                    db.Clients.Update(client);
                    await db.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest(new { errorText = "Такого клиента не существует" });
            }

            return BadRequest(new { errorText = "Ошибка в обработке данных {0}", 
                ModelState.Values });
        }

        /// <summary>
        /// Удаление клиента. 
        /// </summary>
        /// <param name="clientId" example="1">
        /// Client ID
        /// </param>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE /api/deleteclient/1
        ///     
        /// </remarks>
        /// <response code="200">Возвращает успешный результат удаления клиента</response>
        /// <response code="400">Возвращает сообщение об ошибке обработки данных </response>
        [HttpDelete]
        [Route("/api/deleteclient/{clientId}")]
        public async Task<IActionResult> DeleteClient([Required] string clientId)
        {
            if (int.TryParse(clientId, out int id))
            {
                Client client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId == id);

                if (client is not null)
                {
                    db.Clients.Remove(client);
                    await db.SaveChangesAsync();
                    
                    return Ok();
                }

                return BadRequest(new { errorText = "Такого клиента не существует" });
            }

            return BadRequest(new { errorText = "Ошибка в обработке данных" });
        }
    }
}
