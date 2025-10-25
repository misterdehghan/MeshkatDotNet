using AutoMapper;
using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.Assessment.Dto;
using Azmoon.Application.Service.WorkPlace.Dto;
using Azmoon.Application.Service.WorkPlace.Query;
using Azmoon.Common.ResultDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmoon.Application.Service.Assessment
    {
    public interface IGetAssessmentUserAccessWorkPalce
        {
        UserAccessWorkPlacesTreeViewDto GetTreeViewDto(string userName);
        }
    public class GetAssessmentUserAccessWorkPalce : IGetAssessmentUserAccessWorkPalce
        {
        private readonly IDataBaseContext _context;
        private readonly IMapper _mapper;


        public GetAssessmentUserAccessWorkPalce(IDataBaseContext context, IMapper mapper)
            {
            _context = context;
            _mapper = mapper;

            }
        public UserAccessWorkPlacesTreeViewDto GetTreeViewDto(string userName)
            {
            GetChildrenWorkPlacees _getChildrenWorkPlaces = new GetChildrenWorkPlacees(_context);
            var userAssecc = _context.UserAccess.Where(p => p.UserName == userName).FirstOrDefault();

            if (userAssecc != null)
                {
                long par = (long)userAssecc.WorkPlaceId;
                var getChildre = _getChildrenWorkPlaces.ExequteById(par);
                var result = _context.WorkPlaces.Where(p => getChildre.Data.Contains(p.Id)).AsQueryable();
                if (result != null)
                    {
                    var model = _mapper.ProjectTo<GetWorkPlaceViewModel>(result).ToList();

                    for (int i = 0; i < model.Count(); i++)
                        {
                        if (_context.WorkPlaces.Where(p => p.ParentId == model[i].Id).Any())
                            {
                            model[i].IsChailren = true;
                            }
                        }
                    return new UserAccessWorkPlacesTreeViewDto()
                        {

                        wpId = userAssecc.WorkPlaceId,
                        getWorksPlaces = model
                        };
                    }
                }

            return new UserAccessWorkPlacesTreeViewDto()
                {

                wpId = userAssecc.WorkPlaceId,
                getWorksPlaces = null
                };
            }
        }
    }
  
