using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platforms")]
public class PlatformsController : ControllerBase
{
    private readonly ILogger<PlatformsController> _logger;
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(ILogger<PlatformsController> logger, 
        IPlatformRepo platformRepo, 
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _logger = logger;
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _platformRepo.GetAllPlatform();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms)); 
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatformById(int id)
    {
        var platform = _platformRepo.GetPlatformById(id);

        if (platform == null)
            return NotFound();

        return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);

        _platformRepo.CreatePlatform(platform);

        if (_platformRepo.SaveChanges())
        {
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            try
            {
                await _commandDataClient.SendPlatformCommand(platformReadDto);
            }
            catch (Exception)
            {
                Console.WriteLine($"--> fail to sent message to command service");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platformReadDto);
        } 

        return NotFound();
    }
}
