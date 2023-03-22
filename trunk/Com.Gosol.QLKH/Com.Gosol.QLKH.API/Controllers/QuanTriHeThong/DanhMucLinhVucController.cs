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
     * api/v1/DanhMucLinhVuc
     */
    [Route("api/v1/DanhMucLinhVuc")]
    [ApiController]
    public class DanhMucLinhVucController : BaseApiController
    {
        private IDanhMucLinhVucBUS _DanhMucLinhVucBUS;
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        public DanhMucLinhVucController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IDanhMucLinhVucBUS DanhMucLinhVucBUS, ISystemConfigBUS _SystemConfigBUS, ILogHelper _LogHelper, ILogger<DanhMucLinhVucController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucLinhVucBUS = DanhMucLinhVucBUS;
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
        //[CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Read)]
        public IActionResult GetAll(int? type, string keyword)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_GetALL, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<DanhMucLinhVucModel> Data;
                     Data = _DanhMucLinhVucBUS.GetAll(type, keyword, null);
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
        //[CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Read)]
        public IActionResult GetAllAndGroup(int? type, string keyword, bool? status)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_GetALL, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucLinhVucModel> Data;
                    Data = _DanhMucLinhVucBUS.GetAllAndGroup(type, keyword, status);
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
        //[CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Read)]
        public IActionResult NhomNguoiDung_GetByID(int Id)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_GetByID, EnumLogType.GetList, () =>
                {
                    var data = _DanhMucLinhVucBUS.GetByID(Id);
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
        [CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Create)]
        public IActionResult DanhMucLinhVuc_Insert(DanhMucLinhVucModel item)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_Insert, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucLinhVucBUS.Insert(item);
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
        [CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Create)]
        public IActionResult DanhMucLinhVuc_InsertMultil([FromBody] List<DanhMucLinhVucModel> items)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_InsertMulti, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucLinhVucBUS.InsertMulti(items);
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
        [CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Create)]
        public IActionResult DanhMucLinhVuc_Update(DanhMucLinhVucModel item)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_Update, EnumLogType.Insert, () =>
                {
                    var Result = _DanhMucLinhVucBUS.Update(item);
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
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.dm_linh_vuc, AccessLevel.Delete)]
        public IActionResult DanhMucLinhVuc_Delete([FromBody] List<int> ids)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_LinhVuc_Delete, EnumLogType.Delete, () =>
                {
                    var Result = _DanhMucLinhVucBUS.Delete(ids.FirstOrDefault());
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