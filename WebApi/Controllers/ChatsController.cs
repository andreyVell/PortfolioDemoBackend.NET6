using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Chat;
using Services.Models.ChatMember;
using WebApi.Controllers.Base;
using WebApi.DTOs.Chat;
using WebApi.DTOs.ChatMember;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;

        public ChatsController(IMapper mapper, IChatService chatService)
        {
            _mapper = mapper;
            _chatService = chatService;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(GetChatModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromQuery]string eventOriginConnectionId, [FromBody] CreateChatRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateChatModel>(newRequest);
                return Ok(await _chatService.CreateAsync(model, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("Page"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetChatModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "")
        {
            try
            {
                return Ok(await _chatService.GetPageAsync(startIndex, itemsPerPage, filterString));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{chatId}/ChatSmallAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatSmallAvatarAsync([FromRoute] Guid chatId)
        {
            try
            {
                return Ok(await _chatService.GetChatSmallAvatarAsync(chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{chatId}/ChatBigAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatBigAvatarAsync([FromRoute] Guid chatId)
        {
            try
            {
                return Ok(await _chatService.GetChatBigAvatarAsync(chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPut, Authorize]
        [ProducesResponseType(typeof(SuccessfullUpdateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromQuery] string eventOriginConnectionId, [FromBody] UpdateChatRequest request)
        {
            try
            {
                var updateModel = _mapper.Map<UpdateChatModel>(request);
                return Ok(await _chatService.UpdateAsync(updateModel, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("ChatForInterlocutor"), Authorize]
        [ProducesResponseType(typeof(GetChatModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonalChatForInterlocutorAsync([FromBody] CreateChatMemberForNewChatRequest interlocutor)
        {
            try
            {
                var model = _mapper.Map<CreateChatMemberForNewChatModel>(interlocutor);
                return Ok(await _chatService.GetPersonalChatForInterlocutorAsync(model));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{chatId}"), Authorize]
        [ProducesResponseType(typeof(GetChatModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid chatId)
        {
            try
            {
                return Ok(await _chatService.GetAsync(chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{chatId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid chatId)
        {
            try
            {
                await _chatService.DeleteAsync(chatId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpDelete("{chatId}/DeleteChatMember/{chatMemberId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteChatMemberAsync([FromRoute] Guid chatId, [FromRoute] Guid chatMemberId)
        {
            try
            {
                await _chatService.RemoveChatMemberFromChatAsync(chatId, chatMemberId);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("{chatId}/AddChatMembers"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddChatMembersToChatAsync([FromRoute] Guid chatId,[FromBody] List<CreateChatMemberForNewChatRequest> chatMembers)
        {
            try
            {
                await _chatService.AddChatMembersToChatAsync(chatId, _mapper.Map<List<CreateChatMemberForNewChatModel>>(chatMembers));
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
