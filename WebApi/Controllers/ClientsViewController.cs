using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Authentication;
using Services.Models.Chat;
using Services.Models.ChatMessage;
using Services.Models.Project;
using Services.Models.ProjectStage;
using WebApi.Controllers.Base;
using WebApi.DTOs.Authentication;
using WebApi.DTOs.ChatMessage;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsViewController : ApiControllerBase
    {
        private readonly IClientsViewService _clientViewService;
        private readonly IMapper _mapper;
        public ClientsViewController(
            IClientsViewService clientsViewService,
            IMapper mapper)
        {
            _clientViewService = clientsViewService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginClientRequest request)
        {
            try
            {
                var newUser = _mapper.Map<LoginClientModel>(request);
                var token = await _clientViewService.LoginAsync(newUser);
                return Ok(new LoginUserResponse { Token = token });
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Projects/Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetProjectModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectsPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                //Page?startIndex=0&itemsPerPage=50&filterString=test
                return Ok(await _clientViewService.GetProjectPageForCurrentUserAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("ProjectDetails/{projectId}"), Authorize]
        [ProducesResponseType(typeof(GetProjectModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectDetailsAsync([FromRoute] Guid projectId)
        {
            try
            {
                return Ok(await _clientViewService.GetProjectDetailsAsync(projectId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("GetAllStagesForProject/{projectId}"), Authorize]
        [ProducesResponseType(typeof(List<GetProjectStageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStagesForProjectAsync([FromRoute] Guid projectId, [FromQuery] string? filterString = "")
        {
            try
            {
                return Ok(await _clientViewService.GetAllStagesForProjectAsync(projectId, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("GetProjectStageDetails/{projectStageId}"), Authorize]
        [ProducesResponseType(typeof(GetProjectStageModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectStageDetailsAsync([FromRoute] Guid projectStageId)
        {
            try
            {
                return Ok(await _clientViewService.GetProjectStageDetailsAsync(projectStageId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
        [HttpGet("GetProjectName/{projectStageId}"), Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectNameAsync([FromRoute] Guid projectStageId)
        {
            try
            {
                return Ok(new { Name = await _clientViewService.GetProjectNameForStageAsync(projectStageId) });
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("GetStageReportAttachedFileContent/{stageReportAttachedFileId}"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStageReportAttachedFileContentAsync([FromRoute] Guid stageReportAttachedFileId, [FromQuery] bool isImageMedium = false)
        {
            try
            {
                return Ok(await _clientViewService.GetStageReportAttachedFileContentAsync(stageReportAttachedFileId, isImageMedium));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("ChatMembers/{chatMemberId}/SmallAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatMemberSmallAvatarAsync([FromRoute] Guid chatMemberId)
        {
            try
            {
                return Ok(await _clientViewService.GetChatMemberSmallAvatarAsync(chatMemberId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("ChatMessages"), Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateChatMessageAsync([FromQuery] string eventOriginConnectionId, [FromBody] CreateChatMessageRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateChatMessageModel>(newRequest);
                return Ok(await _clientViewService.CreateMessageAsync(model, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("ChatMessages/LoadMoreMessages"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetChatMessageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessagesForChatAsync([FromQuery] Guid chatId, [FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50)
        {
            try
            {
                return Ok(await _clientViewService.GetMessagesForChatAsync(chatId, startIndex, itemsPerPage));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("ChatMessages/{chatMessageId}/ViewBy/{viewedByChatMemberId}"), Authorize]
        [ProducesResponseType(typeof(GetChatMessageViewedInfoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewMessageAsync([FromRoute] Guid chatMessageId, [FromRoute] Guid viewedByChatMemberId, [FromQuery] string eventOriginConnectionId)
        {
            try
            {
                return Ok(await _clientViewService.ViewMessageAsync(chatMessageId, viewedByChatMemberId, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("ChatMessages/AttachedFiles/{chatMessageAttachedFileId}/FileContent"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatMessageAttachedFileContentAsync([FromRoute] Guid chatMessageAttachedFileId, [FromQuery] bool isImageMedium = false)
        {
            try
            {
                return Ok(await _clientViewService.GetChatMessageAttachedFileContentAsync(chatMessageAttachedFileId, isImageMedium));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Chats/Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetChatModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatsPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                return Ok(await _clientViewService.GetChatsPageAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Chats/{chatId}/ChatSmallAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatSmallAvatarAsync([FromRoute] Guid chatId)
        {
            try
            {
                return Ok(await _clientViewService.GetChatSmallAvatarAsync(chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Chats/{chatId}"), Authorize]
        [ProducesResponseType(typeof(GetChatModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid chatId)
        {
            try
            {
                return Ok(await _clientViewService.GetChatAsync(chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
