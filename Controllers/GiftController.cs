using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GiftController : ControllerBase
{
    private IGiftService GiftService;
    public GiftController(IGiftService GiftService){
        this.GiftService = GiftService;
    }

    [HttpGet]
    [Authorize(Policy = "User")]

    public ActionResult<IEnumerable<Gift>> Get()
    {
        return GiftService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult<Gift> Get(int id)
    {
        var gift = GiftService.Get(id);
        if (gift == null)
            return NotFound();
        return gift;
    }

    [HttpPost]
    [Authorize(Policy = "User")]

    public ActionResult Post(Gift newGift)
    {
        var newId = GiftService.Insert(newGift);
        if (newId == -1)
            return BadRequest();
        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult Put(int id, Gift newGift)
    {
        if(GiftService.Update(id,newGift))
            return NoContent();

        return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult Delete(int id)
    {
        if (GiftService.Delete(id))
            return Ok();
            
        return NotFound();
    }   
}