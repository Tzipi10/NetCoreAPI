// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using MyApi.Interfaces;

// namespace MyApi.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class GenericController<T> : ControllerBase
//     {
//         private readonly IService<T> _service;

//         public GenericController(IService<T> service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public ActionResult<IEnumerable<T>> Get()
//         {
//             return _service.Get();
//         }

//         [HttpGet("{id}")]
//         public ActionResult<T> Get(int id)
//         {
//             var item = _service.Get(id);
//             if (item == null)
//                 return NotFound();
//             return item;
//         }

//         [HttpPost]
//         public ActionResult Post(T newItem)
//         {
//             var newId = _service.Insert(newItem);
//             if (newId == -1)
//                 return BadRequest();
//             return CreatedAtAction(nameof(Post), new { Id = newId });
//         }

//         [HttpPut("{id}")]
//         public ActionResult Put(int id, T newItem)
//         {
//             if (_service.Update(id, newItem))
//                 return NoContent();

//             return BadRequest();
//         }

//         [HttpDelete("{id}")]
//         public ActionResult Delete(int id)
//         {
//             if (_service.Delete(id))
//                 return Ok();

//             return NotFound();
//         }
//     }
// }
