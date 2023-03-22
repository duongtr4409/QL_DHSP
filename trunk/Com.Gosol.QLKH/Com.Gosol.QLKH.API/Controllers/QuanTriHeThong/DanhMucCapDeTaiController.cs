using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.Security;
using Microsoft.Extensions.Logging;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.API.Config;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Extensions.Caching.Memory;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    /*
     * api/v1/DanhMucCapDeTai
     */
    [Route("api/v1/DanhMucCapDeTai")]
    [ApiController]
    public class DanhMucCapDeTaiController : BaseApiController
    {
        private IDanhMucCapDeTaiBUS _DanhMucCapDeTaiBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        public DanhMucCapDeTaiController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IDanhMucCapDeTaiBUS DanhMucCapDeTaiBUS, ISystemConfigBUS _SystemConfigBUS, ILogHelper _LogHelper, ILogger<DanhMucCapDeTaiController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucCapDeTaiBUS = DanhMucCapDeTaiBUS;
            this._SystemConfigBUS = _SystemConfigBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
        }

        /// <summary>
        /// Lấy danh sách cấp đề tài
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        //[CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Read)]
        public IActionResult GetAll(string keyword)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_GetALL, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<DanhMucCapDeTaiModel> Data;
                     Data = _DanhMucCapDeTaiBUS.GetAll(keyword);
                     base.Status = 1;
                     base.TotalRow = TotalRow;
                     base.Data = Data;
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
        [Route("GetAllAndGroup")]
        //[CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Read)]
        public IActionResult GetAllAndGroup(string keyword, bool? status)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_GetALL, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucCapDeTaiModel> Data;
                    Data = _DanhMucCapDeTaiBUS.GetAllAndGroup(keyword, status);
                    base.Status = 1;
                    base.TotalRow = TotalRow;
                    base.Data = Data;
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
        [Route("GetByID")]
        //[CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Read)]
        public IActionResult NhomNguoiDung_GetByID(int Id)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_GetByID, EnumLogType.GetList, () =>
                {
                    var data = _DanhMucCapDeTaiBUS.GetByID(Id);
                    base.Status = 1;
                    base.Data = data;
                    base.Message = "";
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
        [Route("Insert")]
        [CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Create)]
        public IActionResult DanhMucCapDeTai_Insert(DanhMucCapDeTaiModel item)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_Insert, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucCapDeTaiBUS.Insert(item);
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
        [Route("InsertMultil")]
        [CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Create)]
        public IActionResult DanhMucCapDeTai_InsertMultil([FromBody] DanhMucCapDeTaiPartialModel items)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_InsertMulti, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucCapDeTaiBUS.InsertMulti(items.items);
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
        [Route("Update")]
        [CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Create)]
        public IActionResult DanhMucCapDeTai_Update(DanhMucCapDeTaiModel item)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_Update, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucCapDeTaiBUS.Update(item);
                    if (Result.Status > 0)
                        base.Status = 1;
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
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.dm_cap_de_tai, AccessLevel.Delete)]
        public IActionResult DanhMucCapDeTai_Delete([FromBody] List<int> ids)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_Delete, EnumLogType.Delete, () =>
                {
                    var Result = _DanhMucCapDeTaiBUS.Delete(ids.FirstOrDefault());
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










    }

}