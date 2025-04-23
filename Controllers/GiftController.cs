using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Interfaces;

namespace MyApi.Controllers
{
    [Route("gift")]
    public class GiftController : GenericController<Gift>
    {
        public GiftController(IGiftService giftService) : base(giftService)
        {
        }
    }
}
