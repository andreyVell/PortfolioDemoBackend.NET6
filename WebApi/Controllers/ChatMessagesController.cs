using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ChatMessage;
using WebApi.Controllers.Base;
using WebApi.DTOs.ChatMessage;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatMessageService _chatMessageService;

        public ChatMessagesController(IMapper mapper, IChatMessageService chatMessageService)
        {
            _mapper = mapper;
            _chatMessageService = chatMessageService;
        }

        [HttpPost, Authorize]
        [ProducesResponseType(typeof(SuccessfullCreateModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateChatMessageAsync([FromQuery] string eventOriginConnectionId, [FromBody] CreateChatMessageRequest newRequest)
        {
            try
            {
                var model = _mapper.Map<CreateChatMessageModel>(newRequest);
                return Ok(await _chatMessageService.CreateAsync(model, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("LoadMoreMessages"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetChatMessageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMessagesForChatAsync([FromQuery] Guid chatId, [FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50)
        {
            try
            {
                return Ok(await _chatMessageService.GetMessagesForChatAsync(chatId, startIndex, itemsPerPage));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost("{chatMessageId}/ViewBy/{viewedByChatMemberId}"), Authorize]
        [ProducesResponseType(typeof(GetChatMessageViewedInfoModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> ViewMessageAsync([FromRoute] Guid chatMessageId, [FromRoute] Guid viewedByChatMemberId, [FromQuery] string eventOriginConnectionId)
        {
            try
            {
                return Ok(await _chatMessageService.ViewMessageAsync(chatMessageId, viewedByChatMemberId, eventOriginConnectionId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("{chatMessageAttachedFileId}/FileContent"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFileContentAsync([FromRoute] Guid chatMessageAttachedFileId, [FromQuery] bool isImageMedium = false)
        {
            try
            {
                return Ok(await _chatMessageService.GetChatMessageAttachedFileContentAsync(chatMessageAttachedFileId, isImageMedium));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
