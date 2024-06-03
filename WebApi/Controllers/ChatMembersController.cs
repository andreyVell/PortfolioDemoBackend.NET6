using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ChatMember;
using WebApi.Controllers.Base;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMembersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatMemberService _chatMemberService;

        public ChatMembersController(IMapper mapper, IChatMemberService chatMemberService)
        {
            _mapper = mapper;
            _chatMemberService = chatMemberService;
        }

        [HttpGet("{chatMemberId}/SmallAvatar"), Authorize]
        [ProducesResponseType(typeof(AttachFileModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChatMemberSmallAvatarAsync([FromRoute] Guid chatMemberId)
        {
            try
            {
                return Ok(await _chatMemberService.GetChatMemberSmallAvatarAsync(chatMemberId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpGet("PotentialMembersPage"), Authorize]
        [ProducesResponseType(typeof(PageModel<GetChatMemberModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPotentialMembersPageAsync([FromQuery] int startIndex = 0, [FromQuery] int itemsPerPage = 50, [FromQuery] string? filterString = "", [FromQuery] Guid? chatId = null)
        {
            try
            {
                return Ok(await _chatMemberService.GetPotentialChatMembersForChatAsync(startIndex, itemsPerPage, filterString, chatId));
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }
    }
}
