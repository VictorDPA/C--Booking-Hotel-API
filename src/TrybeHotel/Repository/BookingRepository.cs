using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var room = _context.Rooms.Find(booking.RoomId);
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            if (room.Capacity >= booking.GuestQuant)
            {
                var newBooking = new Booking
                {
                    CheckIn = DateTime.Parse(booking.CheckIn),
                    CheckOut = DateTime.Parse(booking.CheckOut),
                    GuestQuant = booking.GuestQuant,
                    RoomId = booking.RoomId,
                    UserId = user.UserId
                };
                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                var content = from b in _context.Bookings
                              orderby b.BookingId
                              select new BookingResponse
                              {
                                  bookingId = b.BookingId,
                                  CheckIn = b.CheckIn.ToString("yyyy-MM-dd"),
                                  CheckOut = b.CheckOut.ToString("yyyy-MM-dd"),
                                  guestQuant = b.GuestQuant,
                                  room = new RoomDto
                                  {
                                      roomId = b.Room.RoomId,
                                      name = b.Room.Name,
                                      capacity = b.Room.Capacity,
                                      image = b.Room.Image,
                                      hotel = new HotelDto
                                      {
                                          hotelId = b.Room.Hotel.HotelId,
                                          name = b.Room.Hotel.Name,
                                          address = b.Room.Hotel.Address,
                                          cityId = b.Room.Hotel.CityId,
                                          cityName = b.Room.Hotel.City.Name,
                                          state = b.Room.Hotel.City.State,
                                      }
                                  }
                              };
                return content.Last();
            }
            return null;
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            var booking = _context.Bookings.Find(bookingId);

            if (booking == null || booking.UserId != user.UserId)
            {
                return null;
            }
            else
            {
                var content = from b in _context.Bookings
                              where b.BookingId == bookingId
                              select new BookingResponse
                              {
                                  bookingId = b.BookingId,
                                  CheckIn = b.CheckIn.ToString("yyyy-MM-dd"),
                                  CheckOut = b.CheckOut.ToString("yyyy-MM-dd"),
                                  guestQuant = b.GuestQuant,
                                  room = new RoomDto
                                  {
                                      roomId = b.Room.RoomId,
                                      name = b.Room.Name,
                                      capacity = b.Room.Capacity,
                                      image = b.Room.Image,
                                      hotel = new HotelDto
                                      {
                                          hotelId = b.Room.Hotel.HotelId,
                                          name = b.Room.Hotel.Name,
                                          address = b.Room.Hotel.Address,
                                          cityId = b.Room.Hotel.CityId,
                                          cityName = b.Room.Hotel.City.Name,
                                          state = b.Room.Hotel.City.State,
                                      }
                                  }
                              };
                return content.First();
            }
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}