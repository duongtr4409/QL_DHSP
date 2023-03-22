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

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucLoaiKetQua")]
    [ApiController]
    public class DanhMucLoaiKetQuaController : BaseApiController
    {
        private IDanhMucLoaiKetQuaBUS _DanhMucLoaiKetQuaBUS;
        public DanhMucLoaiKetQuaController(IDanhMucLoaiKetQuaBUS DanhMucLoaiKetQuaBUS, ILogHelper _LogHelper, ILogger<DanhMucLoaiKetQuaController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucLoaiKetQuaBUS = DanhMucLoaiKetQuaBUS;
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
                int TotalRow = 0;
                IList<DanhMucLoaiKetQuaModel> Data;
                Data = _DanhMucLoaiKetQuaBUS.GetAll(keyword);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
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
                int TotalRow = 0;
                IList<DanhMucLoaiKetQuaModel> Data;
                Data = _DanhMucLoaiKetQuaBUS.GetAllAndGroup(keyword, status);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
                return base.GetActionResult();
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
                var data = _DanhMucLoaiKetQuaBUS.GetByID(Id);
                base.Status = 1;
                base.Data = data;
                base.Message = "";
                return base.GetActionResult();
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
        public IActionResult DanhMucLoaiKetQua_Insert(DanhMucLoaiKetQuaModel item)
        {
            try
            {
                var Result = _DanhMucLoaiKetQuaBUS.Insert(item);
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
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
        public IActionResult DanhMucLoaiKetQua_InsertMultil([FromBody] DanhMucLoaiKetQuaPartialModel items)
        {
            try
            {
                var Result = _DanhMucLoaiKetQuaBUS.InsertMulti(items.items);
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
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
        public IActionResult DanhMucLoaiKetQua_Update(DanhMucLoaiKetQuaModel item)
        {
            try
            {
                var Result = _DanhMucLoaiKetQuaBUS.Update(item);
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
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
        public IActionResult DanhMucLoaiKetQua_Delete([FromBody] List<int> ids)
        {
            try
            {
                var Result = _DanhMucLoaiKetQuaBUS.Delete(ids.FirstOrDefault());
                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
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