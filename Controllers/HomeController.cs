using EventMangmentSystem.Data;
using EventMangmentSystem.Models;
using EventMangmentSystem.Services;
using EventMangmentSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace EventMangmentSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [Authorize]
        [HttpGet("{genreId?}")]
        public async Task<IActionResult> Index(int? genreId, int? page, int pageSize = 2)
        {
            int pageNumber = page ?? 1;
            int skip = (pageNumber - 1) * pageSize;

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var eventCountGoingUser = new EventCountGoingUser(context);
            ViewBag.EventCount = eventCountGoingUser;

            // Get total count of events after applying filters
            IQueryable<Event> eventsQuery = context.Events.OrderByDescending(d => d.Date).Include(g => g.Genre);
            if (genreId.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.GenreId == genreId);
            }
            int totalEvents = await eventsQuery.CountAsync();

            // Get events for the current page
            var events = await eventsQuery.Skip(skip).Take(pageSize).ToListAsync();

            // Get genres for dropdown
            var genres = await context.Genres.ToListAsync();

            var viewModel = new EventViewModel
            {
                Events = events,
                CurrentUserId = currentUserId,
                SelectedGenreId = genreId,
                Genres = genres,
                PageSize = pageSize,
                TotalEvents = totalEvents,
                CurrentPage = pageNumber
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
