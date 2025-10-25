using System;

namespace EndPoint.Site.Areas.PublicRelations.Models.Dto.Operator
{
    public class PostOperatorEditDto
    {
        public string UserId { get; set; }
        public Guid Operator { get; set; }
        public string OperatorName { get; set; }
    }
}
