using EventMangmentSystem.Data;

namespace EventMangmentSystem.Services
{
    public  class EventCountGoingUser
    {
        private readonly ApplicationDbContext context;

        public EventCountGoingUser(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int GetCount(int eventId)
        {
            var count = context.UserEvents.Count(ue => ue.EventId == eventId);
            return count;
        }

    }
}
