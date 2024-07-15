using Abp.Application.Services.Dto;

namespace FriendsAndDebt.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

