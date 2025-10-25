using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.JameiatQustion.Dto;
using Azmoon.Application.Service.WorkPlace.Dto;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Common.ResultDto;
using Azmoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.JameiatQustion.Command
    {
    public interface IAddJameiatQustion
        {

        ResultDto<List<GetJameiatQustionViewModel>> GetJameiatQustion(int? parentId);
        ResultDto<Domain.Entities.Template.JameiatQustion> AddJameiat(AddJameiatQustionDto dto);
        }
    public class AddJameiatQustion  : IAddJameiatQustion
        {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;

        public AddJameiatQustion(IDataBaseContext context, IMapper mapper)
            {
            _context = context;
            _mapper = mapper;
            }

        public ResultDto<List<GetJameiatQustionViewModel>> GetJameiatQustion(int? parentId)
            {
           
            var result = _context.jameiatQustions.AsQueryable();
            if (parentId == null)
                {

                result = result.Where(p => p.ParentId == parentId).AsQueryable();
                }
            if (parentId != null)
                {
                int par = (int)parentId;
               // var getChildre = GetChildrensExequteById(par);
                result = result.Where(p => p.ParentId==par).AsQueryable();
                }

            if (result != null)
                {
                var model = _mapper.ProjectTo<GetJameiatQustionViewModel>(result).ToList();

                for (int i = 0; i < model.Count(); i++)
                    {
                    if (_context.jameiatQustions.Where(p => p.ParentId == model[i].Id).Any())
                        {
                        model[i].IsChailren = true;
                        }
                    }
                return new ResultDto<List<GetJameiatQustionViewModel>>
                    {
                    Data = model,
                    IsSuccess = true,
                    Message = "موفق"
                    };
                }
            return new ResultDto<List<GetJameiatQustionViewModel>>
                {
                Data = null,
                IsSuccess = false,
                Message = "ناموفق"
                };
            }

        public ResultDto<Domain.Entities.Template.JameiatQustion> AddJameiat(AddJameiatQustionDto dto)
            {
            var jameiat = _mapper.Map<Domain.Entities.Template.JameiatQustion>(dto);
            if (dto.Id > 0)
                {

                jameiat.Id = dto.Id;
                _context.jameiatQustions.Update(jameiat);
                }
            else
                {
                jameiat.RegesterAt = DateTime.Now;
                _context.jameiatQustions.Add(jameiat);
                }

            var result = _context.SaveChanges();
            if (result > 0)
                {
                return new ResultDto<Domain.Entities.Template.JameiatQustion>
                    {
                    Data = jameiat,
                    IsSuccess = true,
                    Message = "اقدام موفق بود"
                    };
                }
            return new ResultDto<Domain.Entities.Template.JameiatQustion>
                {
                Data = null,
                IsSuccess = false,
                Message = "اقدام با خطا مواجه شد"
                };
            }

        public ResultDto<List<int>> GetChildrensExequteById(int? Id)
            {
            var categores = _context.jameiatQustions.Where(p => p.Id == Id).Select(p => p.Id).ToList();
            List<int> resultList = new List<int>();
            resultList.AddRange(categores);

            foreach (var item in categores)
                {
                getChildren(item, resultList);
                }
            return new ResultDto<List<int>>
                {
                Data = resultList,
                IsSuccess = true,
                Message = "موفق"
                };
            }
        private List<int> getChildren(int id, List<int> result)
            {

            var query = _context.jameiatQustions.Where(p => p.ParentId == id).Select(p => p.Id).ToList();
            if (query != null && query.Count() > 0)
                {
             
                EqulesTwoListAndAppend(query, result);
                foreach (var item in query)
                    {
                    if (_context.WorkPlaces.Where(p => p.ParentId == item).Any())
                        {

                        getChildren(item, result);
                        //return result;
                        }
                    }
                return result;
                }
            return result;
            }
        private List<int> EqulesTwoListAndAppend(List<int> src, List<int> dec)
            {

            foreach (var item in src)
                {
                if (!dec.Where(p => p == item).Any())
                    {
                    dec.Add(item);
                    }
                }
            return dec;
            }
        }
    }
