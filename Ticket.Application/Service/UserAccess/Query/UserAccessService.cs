using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.UserAccess.Dto;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.UserAccess.Query
    {
    public class UserAccessService : IUserAccessService
        {
        private readonly IDataBaseContext _context;

        public UserAccessService(IDataBaseContext context)
            {
            _context = context;
            }

        public ResultDto<int> Add(long wpId , string username , string creatorUserName)
            {
           var aceess = _context.UserAccess.Where(p=>p.UserName==username).FirstOrDefault();
            if (aceess!=null)
            {
                aceess.WorkPlaceId=wpId;
                _context.SaveChanges();
                }
            else
            {
                var acess = new Domain.Entities.Template.UserAccess() { 
                       CreatorUserName= creatorUserName,
                       WorkPlaceId=wpId,
                       UserName=username,
                    };
                _context.UserAccess.Add(acess);
                _context.SaveChanges();
            }
            return new ResultDto<int>() { 
                Data = 0,
                IsSuccess=true,
                    Message="موفق"
                
                };
        }
            
        public ResultDto<string> DeleteAccess(int id)
            {
           var aceess = _context.UserAccess.Where(p=>p.Id==id).FirstOrDefault();
            if (aceess!=null)
            {
                _context.UserAccess.Remove(aceess);
                _context.SaveChanges();
                return new ResultDto<string>() { 
                    Data=aceess.UserName ,
                    IsSuccess=true,
                    };
                }
        
            return new ResultDto<string>() { 
                
                IsSuccess=false,
                    Message="دسترسی یافت نگردید"
                
                };
        }

        public ResultDto<AddUserAccessDto> Getes(string username)
            {
            var aceess = _context.UserAccess.AsNoTracking().Where(p => p.UserName == username).FirstOrDefault();
           
            if (aceess != null) {
                var wp = _context.WorkPlaces.AsNoTracking().Where(p => p.Id == aceess.WorkPlaceId).FirstOrDefault();
                var user = _context.Users.Where(p => p.UserName == username).FirstOrDefault();
                var model = new AddUserAccessDto() { 
                    FullName = user.FirstName + "  " + user.LastName,
                    Id=aceess.Id,
                    UserName= username,
                    WorkPlaceId=aceess.WorkPlaceId  ,
                    WorkPlaceName=wp.Name
                    
                    };
                return new ResultDto<AddUserAccessDto>()
                    {
                    Data = model,
                    IsSuccess = true,
                    };
                }
            return new ResultDto<AddUserAccessDto>() {
                IsSuccess=false,
                };
            }
        }
    }
