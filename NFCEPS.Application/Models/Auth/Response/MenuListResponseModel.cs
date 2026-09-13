using NFCEPS.Domain.Models;

namespace NFCEPS.Application.Models.Auth.Response
{
    public class MenuListResponseModel : StatusResponse
    {
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public int ParentId { get; set; }
        public string? Icon { get; set; }
        public string? Path { get; set; }
        public int MenuOrder { get; set; }
        public int? ChildId { get; set; }
    }
}


