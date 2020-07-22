using AutoMapper;
using GrupoCinte.Api.Data;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Entities;
using GrupoCinte.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthRepository authRepository;
        private readonly IMapper Mapper;
        public UsersController(IUserRepository userRepository, IAuthRepository authRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.authRepository = authRepository;
            Mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await userRepository.GetUsers();
                var userToReturn = Mapper.Map<IEnumerable<UserForDetailedDto>>(users);
                return StatusCode(200, userToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Get users error", ex));
            }
        }

        [HttpGet("GetUserIdTypes")]
        public async Task<IActionResult> GetUserIdTypes()
        {
            try
            {
                var idTypes = await userRepository.GetUserIdTypes();
                return StatusCode(200, idTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Get users error", ex));
            }
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await userRepository.GetUser(id);
                var userToReturn = Mapper.Map<UserForDetailedDto>(user);
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Get user error", ex));
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserForAddDto userForAddDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userForAddDto.Name))
                {
                    return StatusCode(400, "Please enter a name");
                }
                else if (string.IsNullOrEmpty(userForAddDto.LastName))
                {
                    return StatusCode(400, "Please enter a last name");
                }
                else if (string.IsNullOrEmpty(userForAddDto.IdNumber))
                {
                    return StatusCode(400, "Please enter a id number");
                }
                else if (string.IsNullOrEmpty(userForAddDto.Password))
                {
                    return StatusCode(400, "Please enter a password");
                }
                else if (string.IsNullOrEmpty(userForAddDto.Email))
                {
                    return StatusCode(400, "Please enter a email");
                }
                else if (!Validators.ValidateStringIsNumber(userForAddDto.IdNumber))
                {
                    return StatusCode(400, "Id number should be a number");
                }
                else if (!Validators.ValidateStringEmail(userForAddDto.Email))
                {
                    return StatusCode(400, "Please enter a valid email");
                }
                else if (!Validators.ValidateStringFourToEightCharacters(userForAddDto.Password))
                {
                    return StatusCode(400, "Password should be between 4 and 8 characteres");
                }

                if (await authRepository.UserExists(userForAddDto.IdNumber)) return StatusCode(400, "User already exists");
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userForAddDto.Password, out passwordHash, out passwordSalt);

                var userToCreate = Mapper.Map<User>(userForAddDto);
                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                userRepository.Add(userToCreate);
                if (await userRepository.SaveAll())
                {
                    var userForReturn = Mapper.Map<UserForDetailedDto>(userToCreate);
                    return StatusCode(200, userForReturn);
                }
                return StatusCode(400, "Could not add the user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Add user error", ex));
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserForUpdateDto userForUpdateDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userForUpdateDto.Name))
                {
                    return StatusCode(400, "Please enter a name");
                }
                else if (string.IsNullOrEmpty(userForUpdateDto.LastName))
                {
                    return StatusCode(400, "Please enter a last name");
                }
                else if (string.IsNullOrEmpty(userForUpdateDto.IdNumber))
                {
                    return StatusCode(400, "Please enter a id number");
                }
                else if (string.IsNullOrEmpty(userForUpdateDto.Email))
                {
                    return StatusCode(400, "Please enter a email");
                }
                else if (!Validators.ValidateStringIsNumber(userForUpdateDto.IdNumber))
                {
                    return StatusCode(400, "Id number should be a number");
                }
                else if (!Validators.ValidateStringEmail(userForUpdateDto.Email))
                {
                    return StatusCode(400, "Please enter a valid email");
                }

                var userFromRepo = await userRepository.GetUser(userForUpdateDto.Id);
                if (userFromRepo == null) return StatusCode(404, "Username not found");
                Mapper.Map(userForUpdateDto, userFromRepo);

                if (await userRepository.SaveAll())
                {
                    var userForReturn = Mapper.Map<UserForDetailedDto>(userFromRepo);
                    return StatusCode(200, userForReturn);
                }
                return StatusCode(400, "Could not update the user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Update user error", ex));
            }
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserForUpdateDto userForUpdateDto)
        {
            try
            {
                var userFromRepo = await userRepository.GetUser(userForUpdateDto.Id);
                if (userFromRepo == null) return StatusCode(404, "User not found");

                userRepository.Delete<User>(userFromRepo);
                if (await userRepository.SaveAll())
                {
                    return StatusCode(200, "User deleted");
                }
                return StatusCode(400, "Could not delete the user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, Helpers.Helpers.GetErrorMessage("Delete user error", ex));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                    if (computeHash[i] != passwordHash[i]) return false;
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}