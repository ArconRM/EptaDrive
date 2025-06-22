using EptaDrive.Entities;
using EptaDrive.Entities.Requests;
using EptaDrive.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EptaDrive.Controllers;

[Authorize]
public class UserFileController: Controller
{
    private readonly ILogger<UserFileController> _logger;
    private readonly IUserFileService _userFileService;

    public UserFileController(ILogger<UserFileController> logger, IUserFileService userFileService)
    {
        _logger = logger;
        _userFileService = userFileService;
    }

    [HttpPost(nameof(CreateUserFile))]
    public async Task<IActionResult> CreateUserFile(IFormFile file, long userId, CancellationToken token)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = file.OpenReadStream();
            UserFile userFile = await _userFileService.SaveAsync(stream, file.FileName, userId, token);
            return Ok(userFile);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet(nameof(GetFileAsync))]
    public async Task<IActionResult> GetFileAsync(long userFileId, CancellationToken token)
    {
        try
        {
            var userFileData = await _userFileService.GetFileAsync(userFileId, token);
            return File(userFileData.Stream, userFileData.ContentType, userFileData.FileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpGet(nameof(GetAllUserFiles))]
    public async Task<IActionResult> GetAllUserFiles(long userId, CancellationToken token)
    {
        try
        {
            var file = await _userFileService.GetByUserIdAsync(userId, token);
            return Ok(file);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    [HttpDelete(nameof(DeleteAsync))]
    public async Task<IActionResult> DeleteAsync(long userFileId, CancellationToken token)
    {
        try
        {
            await _userFileService.DeleteAsync(userFileId, token);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}