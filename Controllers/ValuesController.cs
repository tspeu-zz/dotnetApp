using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatinApp.API.Controllers
{
    //http://localhost:5000/api/elNombredelConrollerValuesControllere
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //CONTRUCTOR    ctrl . da opciones en .net
        private readonly DataContext _ctx;
        public ValuesController(DataContext ctx)
        {
            // this._ctx = ctx;
            _ctx = ctx;

        }

        // public ActionResult<IEnumerable<string>> Get()
        //IActionsResult devuelve ademas http response
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            //mostrar execpsiones
            // throw new Exception("test excepticion");

//se pasa el contexto y se devuelve Values. de tipo lista
            //usando sincrosius data, un threat bloqueado
        //    var data = _ctx.Values.ToList();

        //usando async no threat block  ojo importar EntytyFramework
        var data = await _ctx.Values.ToListAsync();

//como devuelve 200 de http se usa ok-y la data en forma de lista
            return Ok(data);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        // public ActionResult<string> Get(int id)
        public async Task<ActionResult> GetData(int id)
        {
            //la interface Datacontext
            var data = await  _ctx.Values.FirstOrDefaultAsync(x =>
                        x.id == id );

            return Ok(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
