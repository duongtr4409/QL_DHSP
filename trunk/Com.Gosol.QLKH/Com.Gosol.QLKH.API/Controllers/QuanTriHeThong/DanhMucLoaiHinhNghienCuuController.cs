using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.Security;
using Microsoft.Extensions.Logging;
using Com.Gosol.QLKH.Models.QLKH;
using Microsoft.Extensions.Options;
using Com.Gosol.QLKH.API.Config;
using Microsoft.Extensions.Caching.Memory;

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucLoaiHinhNghienCuu")]
    [ApiController]
    public class DanhMucLoaiHinhNghienCuuController : BaseApiController
    {
        private IDanhMucLoaiHinhNghienCuuBUS _DanhMucLoaiHinhNghienCuuBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;

        public DanhMucLoaiHinhNghienCuuController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IDanhMucLoaiHinhNghienCuuBUS DanhMucLoaiHinhNghienCuuBUS, ILogHelper _LogHelper, ILogger<DanhMucLoaiHinhNghienCuuController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucLoaiHinhNghienCuuBUS = DanhMucLoaiHinhNghienCuuBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
        }

        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Create)]
        [Route("Insert")]
        public IActionResult Insert(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_Them, EnumLogType.Insert, () =>
                 {
                     var Result = _DanhMucLoaiHinhNghienCuuBUS.Insert(DanhMucLoaiHinhNghienCuuModel);
                     base.Status = Result.Status;
                     base.Message = Result.Message;
                     return base.GetActionResult();
                 });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }

        }

        [HttpPost]
        // [CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Edit)]
        [Route("Update")]
        public IActionResult Update(DanhMucLoaiHinhNghienCuuModel DanhMucLoaiHinhNghienCuuModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_Sua, EnumLogType.Update, () =>
                {
                    var Result = _DanhMucLoaiHinhNghienCuuBUS.Update(DanhMucLoaiHinhNghienCuuModel, Utils.ConvertToInt32(User.Claims.FirstOrDefault(x => x.Type == "CoQuanID").Value, 0));
                    base.Status = Result.Status;
                    base.Message = Result.Message;
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }

        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_Xoa, EnumLogType.Delete, () =>
                {
                    var Result = _DanhMucLoaiHinhNghienCuuBUS.Delete(p.ListID);
                    base.Message = Result.Message;
                    base.Status = Result.Status;
                    return base.GetActionResult();

                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.Message = ConstantLogMessage.API_Error_System;
                return base.GetActionResult();
                throw ex;
            }
        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Read)]
        [Route("GetByID")]
        public IActionResult GetByID(int LoaiHinhNghienCuuID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_GetByID, EnumLogType.GetByID, () =>
                 {
                     var data = new DanhMucLoaiHinhNghienCuuModel();
                     data = _DanhMucLoaiHinhNghienCuuBUS.GetByID(LoaiHinhNghienCuuID);
                     if (data == null)
                     { base.Message = ConstantLogMessage.API_NoData; base.Status = 0; }
                     else { base.Message = " "; base.Status = 1; }
                     base.Data = data;
                     return base.GetActionResult();
                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetListPaging([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<DanhMucLoaiHinhNghienCuuModel> Data;
                     Data = _DanhMucLoaiHinhNghienCuuBUS.GetPagingBySearch(p, ref TotalRow);
                     base.Status = 1;
                     base.TotalRow = TotalRow;
                     base.Data = Data;
                     return base.GetActionResult();
                 });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }
        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_LoaiHinhNghienCuu, AccessLevel.Read)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_LoaiHinhNghienCuu_GetListPaging, EnumLogType.GetList, () =>
                {
                    IList<DanhMucLoaiHinhNghienCuuModel> Data;
                    Data = _DanhMucLoaiHinhNghienCuuBUS.GetAllDangSuDung();
                    base.Status = 1;
                    base.Data = Data;
                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw;
            }

        }




        #region dữ liệu từ core

        [Route("GetCategories")]
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.ca, EnumLogType.GetList, () =>
                //{
                int TotalRow = 0;
                var Data = new List<CategorieModel>();
                var lst = rsApiInCore.core_Categories(0);
                Data.AddRange(lst);
                foreach (var item in lst)
                {
                    Data.AddRange(rsApiInCore.core_Categories(item.Id));
                }

                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
                //});
            }
            catch (Exception ex)
            {
                base.Status = -1;
                return base.GetActionResult();
                throw ex;
            }

        }

        #endregion
    }
}