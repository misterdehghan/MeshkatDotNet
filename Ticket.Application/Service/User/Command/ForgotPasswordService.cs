using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Service.User.Dto;
using Azmoon.Common.ResultDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.User.Command
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IDataBaseContext _context;

        public ForgotPasswordService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<string> forgotPassword(ForgotPasswordDto dto)
        {
            var user = _context.Users.Where(p =>
               p.UserName == dto.personeli.ToString()).FirstOrDefault();
            bool vaild = true;
            var errorText = "";
            if (user!=null)
                {
             
                if (user.melli != dto.melli.Trim())
                    {
                    errorText += " " + "کد ملی صحیح نمی باشد\n";
                    vaild = false;
                    }
           
                if (user.Phone != dto.Phone.Trim())
                    {
                    errorText += " " + "شماره موبایل صحیح نمی باشد\n";
                    vaild = false;
                    }
                if (NormalizePersianChars(user.name_father) != NormalizePersianChars(dto.name_father.Trim()))
                    {
                    errorText += "نام پدر صحیح نمی‌باشد\n";
                    vaild = false;
                    }
                if (vaild)
                    {
                    return new ResultDto<string>
                        {
                        IsSuccess = true,
                        Data =user.Id
                        };
                    }
                else
                    {
                    return new ResultDto<string>
                        {
                        Message = errorText,
                        IsSuccess = false,
                        Data = user.Id
                        };
                    }
                }
 
       
            return new ResultDto<string>
            {
                IsSuccess = false,
                Message = "کاربر با شماره پرسنلی وارد شده یافت نگردید."
            };

        }
        string NormalizePersianChars(string input)
            {
            return input.Replace("ي", "ی")
                        .Replace("ك", "ک")
                        .Replace("ۀ", "ه")
                        .Replace("ة", "ه")
                        .Replace("ؤ", "و")
                        .Replace("إ", "ا")
                        .Replace("أ", "ا")
                        .Replace("آ", "ا")  // اختیاری؛ اگر مقایسه بدون تشدید مدنظر باشه
                        .Replace("ٔ", "")   // حذف همزهٔ ترکیبی
                        .Replace("‌", "")   // حذف نیم‌فاصله (Zero-width non-joiner)
                        .Replace("\u200C", "") // جایگزینی نیم‌فاصله با حذف (در یونیکد)
                        .Replace("\u064B", "") // حذف تنوین فتحه
                        .Replace("\u064C", "") // حذف تنوین ضمه
                        .Replace("\u064D", ""); // حذف تنوین کسره
            }

        }
    }
