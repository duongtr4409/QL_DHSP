﻿using System;
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

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    [Route("api/v1/SystemConfig")]
    [ApiController]
    public class SystemConfigController : BaseApiController
    {
        private ISystemConfigBUS _SystemConfigBUS;
        public SystemConfigController(ISystemConfigBUS SystemConfigBUS, ILogHelper _LogHelper, ILogger<SystemConfigController> logger) : base(_LogHelper, logger)
        {
            this._SystemConfigBUS = SystemConfigBUS;
        }

        [HttpPost]
        [Route("Insert")]
        [CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Create)]
        public IActionResult Insert(SystemConfigModel SystemConfigModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_Them, EnumLogType.Insert, () =>
                {
                    var Result = _SystemConfigBUS.Insert(SystemConfigModel);
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
        [CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Edit)]
        public IActionResult Update(SystemConfigModel SystemConfigModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_Sua, EnumLogType.Update, () =>
                 {
                     var Result = _SystemConfigBUS.Update(SystemConfigModel);
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
        [CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Delete)]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_Xoa, EnumLogType.Delete, () =>
                 {
                     var Result = _SystemConfigBUS.Delete(p.ListID);
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
        [HttpGet]
        [Route("GetByID")]
        // [CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Read)]
        public IActionResult GetByID(int SystemConfigID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_GetByID, EnumLogType.GetByID, () =>
                 {
                     var Data = _SystemConfigBUS.GetByID(SystemConfigID);
                     if (Data == null || Data.SystemConfigID < 1)
                     { base.Message = "Không có Dữ liệu"; base.Status = 0; }
                     else { base.Message = " "; base.Status = 1; }
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
        [Route("GetByKey")]
        //[CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Read)]
        public IActionResult GetByKey(string ConfigKey)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_GetByKey, EnumLogType.GetByName, () =>
                 {
                     var Data = _SystemConfigBUS.GetByKey(ConfigKey);
                     if (Data.SystemConfigID < 1) { base.Message = "Không có Dữ liệu"; base.Status = 0; }
                     else { base.Message = " "; base.Status = 1; }
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
        [Route("GetListPaging")]
        //[CustomAuthAttribute(ChucNangEnum.tham_so, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery]BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_SystemConfig_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<SystemConfigModel> Data;
                     Data = _SystemConfigBUS.GetPagingBySearch(p, ref TotalRow);
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
    }
}