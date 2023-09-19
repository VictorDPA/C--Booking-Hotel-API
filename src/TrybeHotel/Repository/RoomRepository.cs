using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var content = from r in _context.Rooms
                          join h in _context.Hotels on r.HotelId equals h.HotelId
                          join c in _context.Cities on h.CityId equals c.CityId
                          where h.HotelId == HotelId
                          select new RoomDto
                          {
                              roomId = r.RoomId,
                              name = r.Name,
                              capacity = r.Capacity,
                              image = r.Image,
                              hotel = new HotelDto
                              {
                                  hotelId = h.HotelId,
                                  name = h.Name,
                                  address = h.Address,
                                  cityId = c.CityId,
                                  cityName = c.Name,
                                  state = c.State,
                              }
                          };
            return content;
        }

        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var content = from r in _context.Rooms
                          join h in _context.Hotels on r.HotelId equals h.HotelId
                          join c in _context.Cities on h.CityId equals c.CityId
                          where r.RoomId == room.RoomId
                          select new RoomDto
                          {
                              roomId = r.RoomId,
                              name = r.Name,
                              capacity = r.Capacity,
                              image = r.Image,
                              hotel = new HotelDto
                              {
                                  hotelId = h.HotelId,
                                  name = h.Name,
                                  address = h.Address,
                                  cityId = c.CityId,
                                  cityName = c.Name,
                                  state = c.State,
                              }
                          };
            return content.Last();
        }

        public void DeleteRoom(int RoomId)
        {
            var room = _context.Rooms.Find(RoomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
    }
}