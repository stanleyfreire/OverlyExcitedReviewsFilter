using Microsoft.AspNetCore.Mvc;
using ReviewScrapperAPI.Interfaces;
using ReviewScrapperAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewScrapperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsController : Controller
    {
        private readonly IScrapperService _scrapperService;

        public ReviewsController(IScrapperService scrapperService)
        {
            _scrapperService = scrapperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] int pages)
        {
            return Ok(await _scrapperService.FetchReviews(pages));
        }
    }
}
