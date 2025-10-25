using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Service.User.Dto;
using Azmoon.Common.ResultDto;
using AutoMapper;
using Azmoon.Domain.Entities;
using System.Text.RegularExpressions;

namespace Azmoon.Application.Service.User.Command
{
    public class CreateUser : ICreateUser
    {
        private readonly IDataBaseContext _context;
        private readonly Common.FileWork.IFileProvider _fileProvider;
        private readonly UserManager<Domain.Entities.User> _userManger;
        private readonly RoleManager<Domain.Entities.Role> _roleManger;
        private readonly IMapper _mapper;
        public CreateUser(IDataBaseContext context, Common.FileWork.IFileProvider fileProvider, UserManager<Domain.Entities.User> userManger, RoleManager<Domain.Entities.Role> roleManger, IMapper mapper)
        {
            _context = context;
            _fileProvider = fileProvider;
            _userManger = userManger;
            _roleManger = roleManger;
            _mapper = mapper;
        }

        public ResultDto<int> HassUser(long personeli, string melicode)
        {
            if (!validCodeMeli(melicode))
            {
                return new ResultDto<int>
                {
                    Data=1,
                    IsSuccess = false,
                    Message = "کد ملی وارد شده نامعتبر می باشد!!!"
                };
            }
            var ExistUser = _context.Users.AsNoTracking().Where(p => p.UserName == personeli.ToString() || p.melli == melicode).AsNoTracking().FirstOrDefault();

            if (ExistUser != null)
            {
                return new ResultDto<int>
                {
                    Data =2,
                    IsSuccess = false,
                    Message = "کاربر با شماره پرسنلی  یا کد ملی وارد شده قبلا ثبت نام کرده است  !!!"
                };
            }
            return new ResultDto<int>
            {
                Data = 3,
                IsSuccess = true,
                Message = "مجاز به ادامه ثبت نام"
            };

        }
        private bool validCodeMeli(string nationalCode)
        {

            if (String.IsNullOrEmpty(nationalCode))
                return false;


            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (nationalCode.Length != 10)
                return false;
            //در صورتی که کد ملی ده رقم عددی نباشد
            var regex = new Regex(@"\d{10}");
            if (!regex.IsMatch(nationalCode))
                return false;
            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (allDigitEqual.Contains(nationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }
        public ResultDto<Domain.Entities.User> Register(RegisterUserDto dto, IFormFile Image)
        {


            if (dto.Id!=null && dto.Id != "")
            {
                var userexist = _context.Users.AsNoTracking().Where(p => p.Id == dto.Id).FirstOrDefault();
                userexist = _mapper.Map<Domain.Entities.User>(dto);
                userexist.LockoutEnabled = false;
                userexist.SecurityStamp = Guid.NewGuid().ToString();
                _context.SaveChanges();
                return new ResultDto<Domain.Entities.User>
                {
                    Data = userexist,
                    IsSuccess = true,
                    Message = "موفق"
                };
            }
            else
            {
                var ExistUser = _context.Users.AsNoTracking().Where(p => p.UserName == dto.personeli.ToString() || p.melli == dto.melli || p.Phone==dto.Phone).AsNoTracking().FirstOrDefault();
       
                if (ExistUser != null)
                {
                    return new ResultDto<Domain.Entities.User>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "کاربر با شماره پرسنلی ،یا تلفن و یا کد ملی وارد شده موجود می باشد!!!"
                    };
                }
               
              var user = _mapper.Map<Azmoon.Domain.Entities.User>(dto);
                //user.LockoutEnabled = false;
                user.Id = Guid.NewGuid().ToString();

                var result = _userManger.CreateAsync(user, dto.Password).Result;
                if (result.Succeeded)
                {
                 var resultrole=   _userManger.AddToRoleAsync(user, "User").Result;
                    //var person = _mapper.Map<persons>(dto);
                    //_context.Persons.Add(person);
                   var savvved= _context.SaveChanges();
                  
                    return new ResultDto<Domain.Entities.User>
                    {
                        Data = user,
                        IsSuccess = true,
                        Message = "موفق"
                    };
                }
                return new ResultDto<Domain.Entities.User>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "نا موفق رمز فاقد پیچیدگی می باشد"
                };
            }





           

        }
    }
}
