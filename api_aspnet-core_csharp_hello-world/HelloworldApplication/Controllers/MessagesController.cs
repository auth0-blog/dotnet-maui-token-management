using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HelloworldApplication.Models;

namespace HelloworldApplication.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MessagesController : ControllerBase
  {
    private const string publicMessage = "The API doesn't require an access token to share this message.";
    private const string protectedMessage = "The API successfully validated your access token.";
    private const string adminMessage = "The API successfully recognized you as an admin.";

    [HttpGet("public")]
    public ApiResponse GetPublicMessage()
    {
      return new ApiResponse(publicMessage);
    }

    [HttpGet("protected")]
    [Authorize]
    public ApiResponse GetProtectedMessage()
    {
      return new ApiResponse(protectedMessage);
    }

    [HttpGet("admin")]
    [Authorize(Policy = "Admin")]
    public ApiResponse GetAdminMessage()
    {
      return new ApiResponse(adminMessage);
    }
  }
}
