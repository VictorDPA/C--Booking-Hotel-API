using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities.Select(c => new CityDto
            {
                cityId = c.CityId,
                name = c.Name,
                state = c.State,
            });
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            return _context.Cities.Select(c => new CityDto
            {
                cityId = c.CityId,
                name = c.Name,
                state = c.State,
            }).Last();
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            _context.Cities.Update(city);
            _context.SaveChanges();

            var update = from c in _context.Cities
                         where c.CityId == city.CityId
                         select new CityDto
                         {
                             cityId = c.CityId,
                             name = c.Name,
                             state = c.State,
                         };

            return update.First();
        }

    }
}
