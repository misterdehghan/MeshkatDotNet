namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.User
{
    public class GetUserDeleteDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonnelCode { get; set; }
        public bool IsActive { get; set; }
    }
}
