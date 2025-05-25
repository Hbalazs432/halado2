using AutoMapper;
using crypto_s4buby.Context.Context;
using crypto_s4buby.Context.Dtos;
using crypto_s4buby.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_s4buby.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> LoginAsync(UserLoginDto userLoginDto);

    }
    public class UserService : IUserService
    {
        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;

        public UserService(CryptoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) throw new KeyNotFoundException("No user with such ID.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
               // .Include(u => u.Wallet)
               // .ThenInclude(w => w.WalletItems)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _context.Users.Where(u => u.Email == userLoginDto.Email && u.Password == userLoginDto.Password).FirstOrDefaultAsync();

            if (user == null) throw new Exception("Bad Credentials.");

            return true;
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);

            if (CheckUniqueEmail(user.Email)) throw new NotSupportedException("It is not a unique e-mail address.");
            else
            {
                user.Wallet = new Wallet();
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<UserDto>(user);
            }
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("No user found with such ID.");

            if (userUpdateDto.Name != "") user.Name = userUpdateDto.Name;
            if (userUpdateDto.Email != "")
            {
                if (CheckUniqueEmail(userUpdateDto.Email)) throw new NotSupportedException("It is not a unique e-mail address.");
                user.Email = userUpdateDto.Email;
            }
            if (userUpdateDto.Password != "") user.Password = userUpdateDto.Password;
            
            _context.Users.Update(user);
             await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        private bool CheckUniqueEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
