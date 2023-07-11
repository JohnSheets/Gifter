using System;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;
using Gifter.Models;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _UserProfileRepository;
        public UserProfileController(IUserProfileRepository profileRepository)
        {
            _UserProfileRepository = profileRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_UserProfileRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userProfile = _UserProfileRepository.GetById(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }
        [HttpPost]
        public IActionResult Post(UserProfile userProfile)
        {
            _UserProfileRepository.Add(userProfile);
            return CreatedAtAction("Get", new { id = userProfile.Id }, userProfile);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return BadRequest();
            }

            _UserProfileRepository.Update(userProfile);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _UserProfileRepository.Delete(id);
            return NoContent();
        }
    }
}
