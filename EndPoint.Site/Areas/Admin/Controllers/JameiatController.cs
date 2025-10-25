using Azmoon.Application.Interfaces.Contexts;
using Azmoon.Application.Service.JameiatQustion.Command;
using Azmoon.Application.Service.JameiatQustion.Dto;
using Azmoon.Application.Service.WorkPlace.Dto;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace EndPoint.Site.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class JameiatController : Controller
        {
        private readonly IAddJameiatQustion _jameiatQustion;
        private readonly IDataBaseContext _context;
        public JameiatController(IAddJameiatQustion jameiatQustion, IDataBaseContext context)
            {
            _jameiatQustion = jameiatQustion;
            _context = context;
            }

        public IActionResult Index(int? parentId)
            {
            var result = _jameiatQustion.GetJameiatQustion(parentId);
            ViewBag.parentId = parentId;
            return View(result.Data);
            }

        [HttpGet]
        public IActionResult Create(int? parentId)
            {
            AddJameiatQustionDto dto = new AddJameiatQustionDto();
            List<SelectListItem> typeQASelectItem = new List<SelectListItem>();
            typeQASelectItem.Add(new SelectListItem() { Value = "1", Text = "سوال", Selected = true });
            typeQASelectItem.Add(new SelectListItem() { Value = "2", Text = "جواب" });
            ViewData["typeQASelectItem"] = typeQASelectItem;
          List <SelectListItem> ParentList = new List<SelectListItem>();
            ParentList.Add(new SelectListItem() { Value = null, Text = "بدون والد",  Selected = true });
            ParentList.AddRange(_context.jameiatQustions.Where(p => p.Status != 3 && p.ParentId == null).Select(p => new SelectListItem
                {
                Value = p.Id.ToString(),
                Text = p.Name
                }).ToList());
            dto.Parentes = ParentList;
            var referer = HttpContext.Request.Headers["Referer"].ToString();
            dto.referer = referer;
            return View(dto);
            }
        [HttpPost]
        public IActionResult Create(AddJameiatQustionDto placeDto)
            {
            ModelState.Remove("Id"); ModelState.Remove("ParentId");

            if (!ModelState.IsValid)
                {
                return View(placeDto);
                }
            var result = _jameiatQustion.AddJameiat(placeDto);
            if (result.IsSuccess)
                {
                return Redirect(placeDto.referer);
                }
            placeDto.Parentes = _context.jameiatQustions.Where(p => p.Status != 3 && p.ParentId == null).Select(p => new SelectListItem
                {
                Value = p.Id.ToString(),
                Text = p.Name
                }).ToList();
            return View(placeDto);
            }
        [HttpGet]
        public IActionResult Edit(int id)
            {
            List<SelectListItem> typeQASelectItem = new List<SelectListItem>();
            typeQASelectItem.Add(new SelectListItem() { Value = "1", Text = "سوال", Selected = true });
            typeQASelectItem.Add(new SelectListItem() { Value = "2", Text = "جواب" });
            ViewData["typeQASelectItem"] = typeQASelectItem;
            var model = _context.jameiatQustions.Where(p => p.Id == id).Select(p => new AddJameiatQustionDto
                {
                Id = p.Id,
                Name = p.Name,
                ParentId = p.ParentId
                }).FirstOrDefault();
            List<SelectListItem> ParentList = new List<SelectListItem>();
            if (model.ParentId != null) {
                var pa = _context.jameiatQustions.Where(p => p.Id == model.ParentId).FirstOrDefault();
                ParentList.Add(new SelectListItem() { Value = pa.Id.ToString(), Text = pa.Name, Selected = true });
                }
           
            ParentList.Add(new SelectListItem() { Value = null, Text = "بدون والد"});
            ParentList.AddRange(_context.jameiatQustions.Where(p => p.Status != 3 && p.ParentId == null).Select(p => new SelectListItem
                {
                Value = p.Id.ToString(),
                Text = p.Name
                }).ToList());
            model.Parentes = ParentList;
            var referer = HttpContext.Request.Headers["Referer"].ToString();
            model.referer = referer;
            return View("Create", model);
            }
        }
    }
