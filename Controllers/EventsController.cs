using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventMangmentSystem.Data;
using EventMangmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EventMangmentSystem.ViewModel;
using System.Xml.Linq;

namespace EventMangmentSystem.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
       
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim.Value;
            return View(await _context.Events.Where(m=>m.ArtistId==userId).ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id,EventDetailViewModel eventDetail)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Event = await _context.Events.SingleOrDefaultAsync(m => m.Id ==id);
            if (Event == null)
            {
                return NotFound();
            }
           
            
            eventDetail.Event = Event;
            eventDetail.Genre = _context.Genres.Find(Event.GenreId);
            if (eventDetail.Genre == null)
            {
                return NotFound();
            }
            eventDetail.User =  _context.Users.Find(Event.ArtistId);
            return View(eventDetail);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            var ViewModel = new EventFormView { Genres = _context.Genres.ToList() };
            return View(ViewModel);
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(EventFormView model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim.Value;
            var user = _context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {
                // Handle case where user is not found
                // Redirect to an error page or return an appropriate response
                return NotFound();
            }
            var Genre =  _context.Genres.Single(g => g.Id == model.Genre);
            var newEvent = new Event
            {
                Title= model.Title,
                Description= model.Description,
                Artist = user,
                Date = model.Date, // Convert string to DateTime
                Place = model.Place,
                Genre = Genre,
            };

           
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            
          
        }



        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            var viewModel = new EventEditViewModel
            {
                Event = @event,
                Genres = _context.Genres.ToList() 
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Edit(int id, EventEditViewModel model)
        {
            // Find the user
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim.Value;
            var user = _context.Users.SingleOrDefault(x => x.Id == userId);

            if (user == null)
            {

                return NotFound();
            }

            // Find the existing event by ID
            var existingEvent = await _context.Events.FindAsync(id);
           
            var Genre = _context.Genres.Single(g => g.Id == model.Genre);
            if (existingEvent == null)
            {
                return NotFound();
            }

           

            // Update the existing event with the new values
            existingEvent.ArtistId = userId;
            existingEvent.Date = model.Event.Date;
            existingEvent.Place = model.Event.Place;
            existingEvent.Genre = Genre;
            existingEvent.Title = model.Event.Title;
            existingEvent.Description = model.Event.Description;

            try
            {
                 _context.Update(existingEvent);                                                                                                                                   
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Log the error or handle it appropriately
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
                return View(model);
            }
        }


        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                _context.UserEvents.RemoveRange(_context.UserEvents.Where(x => x.EventId == id));
                _context.Comments.RemoveRange(_context.Comments.Where(x => x.EventId == id));

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
     [HttpPost]
     [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsGoing(string userId, int eventId)
    {
            var existingUserEvent = _context.UserEvents.FirstOrDefault(ue => ue.UserId == userId && ue.EventId == eventId);


            if (existingUserEvent != null)
            {
                // Check if the notification message exists in TempData
                if (TempData["NotificationMessage"] == null)
                {
                    TempData["NotificationMessage"] = "You have already marked yourself as going to this event.";
                }
                return RedirectToAction("Index", "Home");
            }
            // Create a new UserEvent object
            var userEvent = new UserEvent
        {
            UserId = userId,
            EventId = eventId
        };

            try
            {
                await _context.UserEvents.AddAsync(userEvent);
                await _context.SaveChangesAsync();
                TempData.Remove("NotificationMessage");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }


    }
        [HttpGet]

        public async Task<IActionResult> EventIwillGo()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim.Value;
            // Retrieve the UserEvents associated with the provided userId
            var userEvents = await _context.UserEvents
                                            .Where(u => u.UserId == userId)
                                            .ToListAsync();

            // Extract the eventIds from the userEvents
            var eventIds = userEvents.Select(ue => ue.EventId).ToList();
            // Retrieve the corresponding events from the Events table
            var events = await _context.Events
                                        .Where(e => eventIds.Contains(e.Id))
                                        .ToListAsync();

            var viewModel = new EventGoingViewModel
            {
                Events = events 
            };

            return View(viewModel); 
        }


        [HttpGet]
        public async Task<IActionResult> AddingCommentToEvent(int eventId)
        {
       
            var eventComments = await _context.Comments
                                              .Include(c => c.User) 
                                              .Where(c => c.EventId == eventId)
                                              .ToListAsync();
            var EventCommentsModel = new AddCommentViewModel { Comments = eventComments, EventId = eventId };

            // Return the view with the view model
            return View(EventCommentsModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingCommentToEvent(AddCommentViewModel model)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim.Value;

            var comment = new Comment
            {
                UserId = userId,
                Text = model.Text,
                EventId = model.EventId,
            };

            _context.Add(comment);
            await _context.SaveChangesAsync();

            // Redirect to the GET action to display the updated comments
            return RedirectToAction(nameof(AddingCommentToEvent), new { eventId = model.EventId });
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
