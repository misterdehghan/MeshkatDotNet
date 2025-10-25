using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService
{
    public interface IChengeStatusUserService
    {
        ResultDto Execute(string UserId);
    }

    public class ChengeStatusUserService : IChengeStatusUserService
    {
        private readonly UserManager<User> _userManager;


        public ChengeStatusUserService(UserManager<User> userManager)
        {
            _userManager = userManager;
     
        }
        public ResultDto Execute(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user==null)
            {
                return new ResultDto(){
                    IsSuccess=false,
                    Message="" 
                };
            }
            else
            {
                user.IsActive = !user.IsActive;
                var result= _userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                {
                    string userstate = user.IsActive == true ? "فعال" : "غیر فعال";
                    return new ResultDto()
                    {
                        IsSuccess = true,
                        Message = $"کاربر با موفقیت {userstate} شد!",
                    };
                }
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "خطایی رخ داده است",
                };
            }
              

        }
    }
}
