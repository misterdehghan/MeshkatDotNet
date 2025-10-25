﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Interfaces.User;
using Azmoon.Application.Service.User.Dto;
using Azmoon.Common.ResultDto;
using Azmoon.Common.Useful;
using Microsoft.EntityFrameworkCore;

namespace Azmoon.Application.Service.User.Query
{
   public class FindUser : IFindUser
    {
        private readonly IDataBaseContext _context;

        public FindUser(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<Domain.Entities.User> Exequte(string id)
        {
            var user = _context.Users.Where(p => p.Id == id).FirstOrDefault();
            if (user!=null)
            {
                return new ResultDto<Domain.Entities.User>
                {
                    Data = user,
                    IsSuccess = true,
                    Message = "موفق"
                };
            }
                return new ResultDto<Domain.Entities.User> 
                { 
                Data=null,
                IsSuccess=false,
                Message="کاربر یافت نگردید!!!!"
                };
        }

        public ResultDto<GetDitalesUserProfileDto> GetPerson(string username)
        {
            var user = _context.Users.Where(p => p.UserName == username)
                .Include(p=>p.WorkPlace)
                .FirstOrDefault();
            var darajehName = "";
            switch (user.TypeDarajeh)
            {
                case 0:
                    darajehName = StaticList.listObjRotbeh().lstoption.Where(p=>p.value==user.darajeh).FirstOrDefault().text;
                    break;
                case 1:
                    darajehName = StaticList.listObjDarajeh().lstoption.Where(p => p.value == user.darajeh).FirstOrDefault().text; 
                    break;
                case 2:
                    darajehName = StaticList.listObjRotbehRoohani().lstoption.Where(p => p.value == user.darajeh).FirstOrDefault().text;
                    break;
                default:
                    break;
            }
            //var person = _context.Persons.Where(p=>p.personeli==user.UserName).FirstOrDefault();
            var dto = new GetDitalesUserProfileDto {
            FirstName=user.FirstName,
            LastName=user.LastName,
            darajehName= darajehName,
                TypeDarajehName = StaticList.listTypeDarajeh.Where(p => p.Value == user.TypeDarajeh).FirstOrDefault().Key,
                Phone =user.Phone,
            WorkplaceName= user.WorkPlace.Name,
            TypeDarajeh = user.TypeDarajeh,
                personId = user.Id,
                darajeh=user.darajeh,
                WorkPlaceId=(long)user.WorkPlaceId,
           userId=user.Id  ,
           NumberBankAccunt=user.NumberBankAccunt
           
            };
            return new ResultDto<GetDitalesUserProfileDto> { 
            Data=dto,
            IsSuccess=true,
            Message="موفق"
            };
        }
    }
}
