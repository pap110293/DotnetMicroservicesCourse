using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        private readonly IMapper _mapper;

        public PlatformsController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            var platforms = await _dbContext.Platforms.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = nameof(GetPlatform))]
        public IActionResult GetPlatform(int id)
        {
            var platform = _dbContext.Platforms.FirstOrDefault(i => i.Id == id);

            if (platform == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<IActionResult>
        CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);

            _dbContext.Platforms.Add (platform);
            await _dbContext.SaveChangesAsync();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            return CreatedAtRoute(nameof(GetPlatform), new { id = platform.Id }, platformReadDto);
        }
    }
}
