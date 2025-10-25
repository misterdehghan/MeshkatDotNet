using Azmoon.Application.Service.Survaeis.Answers;
using Azmoon.Application.Service.UserAccess.Dto;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.UserAccess.Query
    {
    public interface IUserAccessService
        {
        ResultDto<int> Add(long wpId, string username, string creatorId);
        ResultDto<AddUserAccessDto> Getes(string username);
        ResultDto<string> DeleteAccess(int id);
        }
    }
