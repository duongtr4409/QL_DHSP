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
    [Route("api/v1/ChucNang")]
    [ApiController]
    public class ChucNangController : BaseApiController
    {
        private IChucNangBUS _ChucNangBUS;

        public ChucNangController(IChucNangBUS ChucNangBUS, ILogHelper _LogHelper, ILogger<ChucNangController> logger) : base(_LogHelper, logger)
        {
            this._ChucNangBUS = ChucNangBUS;
        }

        [HttpGet]
        [Route("GetListPaging")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_ChucNang, AccessLevel.Read)]
        public IActionResult GetListPaging([FromQuery]BasePagingParamsForFilter p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_ChucNang_GetListPaging, EnumLogType.GetList, () =>
                 {
                     int TotalRow = 0;
                     IList<ChucNangModel> Data;
                     Data = _ChucNangBUS.GetPagingBySearch(p, ref TotalRow);
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