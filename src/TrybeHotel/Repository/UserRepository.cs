using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users
                .FirstOrDefault(
                    u => u.Email == login.Email && u.Password == login.Password);

            if (user != null)
            {
                return new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = user.UserType
                };
            }
            return null;
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.Email.Contains("admin") ? "admin" : "client",
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var contains = from u in _context.Users
                           where u.Email == newUser.Email
                           select new UserDto
                           {
                               UserId = u.UserId,
                               Name = u.Name,
                               Email = u.Email,
                               UserType = u.UserType
                           };
            return contains.Last();
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var show = from u in _context.Users
                       where u.Email == userEmail
                       select new UserDto
                       {
                           UserId = u.UserId,
                           Name = u.Name,
                           Email = u.Email,
                           UserType = u.UserType
                       };
            if (show.Count() > 0)
            {
                return show.First();
            }
            return null;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var content = from u in _context.Users
                          select new UserDto
                          {
                              UserId = u.UserId,
                              Name = u.Name,
                              Email = u.Email,
                              UserType = u.UserType
                          };

            return content;
        }

    }
}