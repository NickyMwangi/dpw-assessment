using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Services;
using Library.Dtos;
using Library.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebUI.Extensions.Helpers;


namespace Web.Controllers
{
    public abstract class BaseController<TEntity, TDto, TDtoLine> : Controller
        where TDto : BaseDto<TDtoLine>, new()
        where TDtoLine : BaseLineDto, new()
        where TEntity : BaseEntity, new()
    {
        protected readonly IRepoService repo;
        protected readonly IMapperService mapper;
        public BaseController(IRepoService _repo, IMapperService _mapper)
        {
            repo = _repo;
            mapper = _mapper;
        }

        public virtual IQueryable<TEntity> FilterQuery(IQueryable<TEntity> query, string filterType)
        {
            return query;
        }

        public virtual TDto DefaultValuesGet(TDto dto, bool isNew, string queryParam)
        {
            return dto;
        }

        public virtual TDto DefaultValuesPost(TDto dto, bool isNew)
        {
            return dto;
        }

        public virtual TDtoLine DefaultValuesLine(TDtoLine dtoLine, bool isNew)
        {
            return dtoLine;
        }

        public virtual TDto PopulateSelectLists(TDto dto)
        {
            return dto;
        }

        public virtual IList<TDtoLine> PopulateSelectLists(IList<TDtoLine> dtoLines)
        {
            if (dtoLines == null)
                dtoLines = new List<TDtoLine>();
            foreach (var dtoline in dtoLines)
            {
                PopulateSelectLists(dtoline);
            }
            return dtoLines;
        }

        public virtual TDtoLine PopulateSelectLists(TDtoLine dtoLine)
        {
            return dtoLine;
        }

        public virtual TEntity LogicAfterPost(TEntity entity, TDto dto)
        {
            return entity;
        }


        public IActionResult RowActionButton(string linkModels)
        {
            var result = JsonConvert.DeserializeObject<List<ActionLinkModel>>(linkModels);
            return PartialView("_RowActionButton", result);
        }

        public virtual IActionResult GetData(string filterType = null)
        {
            var result = repo.Query<TEntity>(false);
            result = FilterQuery(result, filterType);
            var model = mapper.MapConfig<TEntity, TDto>(result).AsEnumerable();
            var data = Json(model);
            return data;
        }

        public virtual async Task<IActionResult> AsyncGetData(string filterType = null)
        {
            var query = repo.Query<TEntity>(false);
            var result = await FilterQuery(query, filterType).ToListAsync();
            var model = mapper.MapConfig<List<TEntity>, List<TDto>>(result);
            var data = Json(model);
            return data;
        }


        public virtual IActionResult Index(string filterParams = null, string menu = null)
        {
            ViewData["controller"] = RouteData.Values["controller"].ToString().ToLower() + filterParams ?? "";
            ViewData["menu"] = menu ?? RouteData.Values["controller"].ToString().ToLower();
            ViewData["filterType"] = filterParams ?? "";
            return View();
        }

        public virtual IActionResult AddEdit(string id = null, string queryParam = null, bool viewOnly = false)
        {
            bool isNew = false;
            var entity = repo.GetById<TEntity>(id ?? Guid.Empty.ToString());
            if (entity == null)
            {
                entity = new TEntity() { Id = Guid.NewGuid().ToString() };
                isNew = true;
            }
            var dto = mapper.MapConfig<TEntity, TDto>(entity);
            dto.NewDto = isNew;
            dto.ViewOnly = viewOnly;
            dto = DefaultValuesGet(dto, isNew, queryParam);
            dto = PopulateSelectLists(dto);
            dto.DtoLines = PopulateSelectLists(dto.DtoLines);
            return View(dto);
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddEdit(TDto dto, string linkAction)
        {
            bool success = false;
            string message;
            string url = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new CustomException("Invalid model");
                }

                var entity = await repo.GetByIdAsync<TEntity>(dto.Id);
                if (entity == null)
                {
                    entity = new TEntity();
                    dto.NewDto = true;
                }
                dto.CreatedOn ??= DateTime.Now;
                dto.CreatedBy ??= User.Identity.Name;
                dto.ModifiedOn = DateTime.Now;
                dto.ModifiedBy = User.Identity.Name;

                dto = DefaultValuesPost(dto, dto.NewDto);
                var model = mapper.MapConfig(dto, entity);
                model = repo.InsertUpdate(model, dto.NewDto);
                await repo.SaveAsync();

                LogicAfterPost(model, dto);

                if (linkAction == "saveNew")
                {
                    url = Url.Action("AddEdit", new { id = "" });
                    message = AlertEnum.success.Toastr_Message("Record Added");
                }
                else
                {
                    url = Url.Action("AddEdit", new { model.Id });
                    message = AlertEnum.success.Swal_Message("Record Saved");
                }
                success = true;
            }
            catch (CustomException ex)
            {
                message = AlertEnum.error.Swal_Message(ex.Message);
            }
            catch (Exception ex)
            {
                message = AlertEnum.error.Swal_Message(ex.Message);
            }

            return Json(new { url, success, message });
        }

        [HttpPost]
        public IActionResult AddLine(TDto dto)
        {
            var line = new TDtoLine()
            {
                LineId = Guid.NewGuid().ToString(),
                HeaderId = dto.Id,
                CreatedOn = DateTime.Now,
                CreatedBy = User.Identity.Name,
                ModifiedOn = DateTime.Now,
                ModifiedBy = User.Identity.Name
            };
            DefaultValuesLine(line, true);
            if (dto.DtoLines == null)
                dto.DtoLines = new List<TDtoLine>();
            dto.DtoLines.Add(line);
            dto.DtoLines = PopulateSelectLists(dto.DtoLines);
            return PartialView("Lines", dto);
        }

        [HttpPost]
        public IActionResult ReloadLines(TDto dto)
        {
            dto.DtoLines = PopulateSelectLists(dto.DtoLines);
            return PartialView("Lines", dto);
        }

        public virtual async Task<IActionResult> Delete(string id)
        {
            bool success = false;
            string message;
            string url;
            try
            {
                var entity = await repo.GetByIdAsync<TEntity>(id);
                if (entity == null)
                    throw new CustomException("Record not found");

                await repo.DeleteAsync(entity);

                url = Url.Action("Index");
                message = AlertEnum.success.Toastr_Message("Record Deleted");
                success = true;
            }
            catch (CustomException ex)
            {
                url = Url.Action("AddEdit", new { id });
                message = AlertEnum.error.Swal_Message(ex.Message);
            }
            catch (Exception ex)
            {
                url = Url.Action("AddEdit", new { id });
                message = AlertEnum.error.Swal_Message(ex.Message);
            }

            return Json(new { url, success, message });
        }

        [HttpPost]
        public IActionResult DeleteLine(TDto dto, string lineId)
        {
            bool success = false;
            string message;
            string url;
            try
            {
                var line = dto.DtoLines.Where(m => m.LineId == lineId);
                if (!line.Any())
                    throw new CustomException("Record not found");
                dto.DtoLines.Remove(line.First());
                message = AlertEnum.success.Swal_Message("Line Deleted. Save to update record");
                success = true;
            }
            catch (CustomException ex)
            {
                message = AlertEnum.error.Swal_Message(ex.Message);
            }
            catch (Exception ex)
            {
                message = AlertEnum.error.Swal_Message(ex.Message);
            }
            url = Url.Action("ReloadLines");
            return Json(new { url, dto, success, message });
        }
    }
}