using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.DanhMuc;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.BUS.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.Gosol.QLKH.API.Controllers.DanhMuc
{
    [Route("api/v1/DanhMucBieuMau")]
    [ApiController]

    public class DanhMucBieuMauController : BaseApiController
    {
        private IDanhMucBieuMauBUS _DanhMucBieuMauBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IHostingEnvironment _host;
        private RestShapAPIInCore rsApiInCore;
        private IOptions<AppSettings> _AppSettings;
        public DanhMucBieuMauController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, IDanhMucBieuMauBUS DanhMucBieuMauBUS, IHostingEnvironment hostingEnvironment, IFileDinhKemBUS FileDinhKemBUS, ILogHelper _LogHelper, ILogger<DanhMucChucVuController> logger) : base(_LogHelper, logger)
        {
            this._DanhMucBieuMauBUS = DanhMucBieuMauBUS;
            this._host = hostingEnvironment;
            this._FileDinhKemBUS = FileDinhKemBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.ql_bieu_mau, AccessLevel.Create)]
        [Route("Insert")]
        public async Task<IActionResult> InsertAsync(IFormFile FileDinhKem, [FromForm] string DanhMucBieuMauModel)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.DM_BieuMau_ThemChucVu, EnumLogType.Insert, async () =>
                //{
                var clsCommon = new Commons();
                var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var BieuMauModel = JsonConvert.DeserializeObject<DanhMucBieuMauModel>(DanhMucBieuMauModel);
                BieuMauModel.NguoiCapNhat = crCanBoID;
                var Result = _DanhMucBieuMauBUS.Insert(BieuMauModel);
                if (Result.Status > 0)
                {
                    if (FileDinhKem != null)
                    {
                        // thêm bảng file đính kèm

                        FileDinhKemModel fileData = new FileDinhKemModel();
                        fileData.LoaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
                        fileData.NguoiTaoID = crCanBoID;
                        fileData.NghiepVuID = Result.Status;
                        fileData.NoiDung = string.Empty;
                        fileData.FolderPath = nameof(EnumLoaiFileDinhKem.DanhMucBieuMau);
                        var insertFile = await clsCommon.InsertFileAsync(FileDinhKem, fileData, _host, _FileDinhKemBUS);
                        if (insertFile)
                        {
                            base.Status = 1;
                            base.Message = Result.Message;
                        }
                        else
                        {
                            base.Status = 0;
                            base.Message = "Không thể tải File";
                        }
                    }
                }
                base.Status = Result.Status > 0 ? 1 : 0;
                base.Message = Result.Message;
                return base.GetActionResult();
                //});
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
        [CustomAuthAttribute(ChucNangEnum.ql_bieu_mau, AccessLevel.Edit)]
        [Route("Update")]
        public async Task<IActionResult> UpdateAsync(IFormFile FileDinhKem, [FromForm] string DanhMucBieuMauModel)
        {
            try
            {
                var BieuMauModel = JsonConvert.DeserializeObject<DanhMucBieuMauModel>(DanhMucBieuMauModel);
                var clsCommon = new Commons();
                var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                BieuMauModel.NguoiCapNhat = crCanBoID;
                var Result = _DanhMucBieuMauBUS.Update(BieuMauModel);
                if (Result.Status > 0)
                {
                    if (FileDinhKem != null)
                    {
                        //delete file đính kèm có sẵn
                        int loaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
                        var ResultDeleteFile = _FileDinhKemBUS.Delete(loaiFile, BieuMauModel.BieuMauID);
                        // thêm bảng file đính kèm sau khi delete thành công
                        if (ResultDeleteFile.Status > 0)
                        {

                            FileDinhKemModel fileData = new FileDinhKemModel();
                            fileData.LoaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
                            fileData.NguoiTaoID = crCanBoID;
                            fileData.NghiepVuID = BieuMauModel.BieuMauID;
                            fileData.NoiDung = string.Empty;
                            fileData.FolderPath = nameof(EnumLoaiFileDinhKem.DanhMucBieuMau);

                            var insertFile = await clsCommon.InsertFileAsync(FileDinhKem, fileData, _host, _FileDinhKemBUS);
                            if (insertFile)
                            {
                                base.Status = 1;
                                base.Message = Result.Message;
                            }
                            else
                            {
                                base.Status = 0;
                                base.Message = "Không thể tải File";
                            }
                        }
                    }
                }
                base.Status = Result.Status > 0 ? 1 : 0;
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

        public FileDinhKemModel GetInfoFileDinhKem(IFormFile FileDinhKem)
        {
            var clsCommon = new Commons();
            FileDinhKemModel fileInfo = new FileDinhKemModel();
            var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
            fileInfo.TenFileGoc = ContentDispositionHeaderValue.Parse(FileDinhKem.ContentDisposition).FileName.Trim('"');
            fileInfo.TenFileGoc = clsCommon.EnsureCorrectFilename(fileInfo.TenFileGoc);
            fileInfo.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + fileInfo.TenFileGoc;
            fileInfo.NguoiTaoID = crCanBoID;
            fileInfo.LoaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
            fileInfo.NgayTao = DateTime.Now;
            fileInfo.FileUrl = clsCommon.GetSavePathFile(_host, fileInfo.TenFileHeThong);
            return fileInfo;
        }

        [HttpPost]
        [CustomAuthAttribute(ChucNangEnum.ql_bieu_mau, AccessLevel.Delete)]
        [Route("Delete")]
        public IActionResult Delete([FromBody] BaseDeleteParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BieuMau_XoaBieuMau, EnumLogType.Delete, () =>
                {

                    var Result = _DanhMucBieuMauBUS.Delete(p.ListID);
                    if (Result.Count > 0)
                    {
                        string Mes = "";
                        foreach (var item in Result)
                        {
                            Mes = string.Concat(Mes, item, ",");
                        };
                        Mes = string.Concat("Bước thực hiện ", Mes, " đang được sử dụng. Không thể xóa!");
                        Mes = Mes.Remove(Mes.LastIndexOf(","), 1);
                        base.Message = Mes;
                        //base.Data = Result;
                        base.Status = 0;
                        return base.GetActionResult();
                    }
                    else
                    {
                        int LoaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
                        for (int i = 0; i < p.ListID.Count; i++)
                        {
                            int BieuMauID = p.ListID[i];
                            _FileDinhKemBUS.Delete(LoaiFile, BieuMauID);
                        }
                        base.Message = "Xóa thành công!";
                        base.Status = 1;
                        return base.GetActionResult();
                    }
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
        [HttpGet]
        [Route("GetByID")]
        //[CustomAuthAttribute(ChucNangEnum.DanhMuc_ChucVu, AccessLevel.Read)]
        public IActionResult GetByID(int BieuMauID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BieuMau_GetByID, EnumLogType.GetByID, () =>
                {
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    var Data = _DanhMucBieuMauBUS.GetByID(BieuMauID);
                    Data.FileDinhKem.FileUrl = serverPath + "\\" + Data.FileDinhKem.FileUrl;
                    if (Data != null && Data.BieuMauID > 0)
                    {
                        base.Message = ConstantLogMessage.API_Success;
                        base.Data = Data;
                    }
                    else
                    {
                        base.Message = ConstantLogMessage.API_NoData;
                    }
                    base.Status = 1;

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
        //[CustomAuthAttribute(ChucNangEnum.ql_bieu_mau, AccessLevel.Read)]
        [Route("GetListPaging")]
        public IActionResult GetListPaging([FromQuery] BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BieuMau_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucBieuMauModel> Data;
                    var clsCommon = new Commons();
                    string serverPath = clsCommon.GetServerPath(HttpContext);
                    Data = _DanhMucBieuMauBUS.GetPagingBySearch(p, ref TotalRow);
                    var dsCanBoInCore = rsApiInCore.core_DSCanBoTheoDonVi(0);
                    foreach (var item in Data)
                    {
                        item.FileDinhKem.FileUrl = serverPath + item.FileDinhKem.FileUrl;
                        if (item.NguoiCapNhat != null)
                        {
                            var NguoiCapNhat = dsCanBoInCore.FirstOrDefault(x => x.Id == item.NguoiCapNhat.Value);
                            if (NguoiCapNhat != null) item.NguoiCapNhatStr = NguoiCapNhat.Name;
                        }
                    }
                    //Data.ToList().ForEach(item => { item.FileDinhKem.FileUrl = serverPath + item.FileDinhKem.FileUrl; item.NguoiCapNhatStr = dsCanBoInCore.FirstOrDefault(x => x.Id == item.NguoiCapNhat.Value).Name; });
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count >= 0 ? 1 : 0;
                    base.TotalRow = TotalRow;
                    base.Data = Data;

                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }

        [HttpGet]
        //[CustomAuthAttribute(ChucNangEnum.ql_bieu_mau, AccessLevel.Read)]
        [Route("GetAll")]
        public IActionResult GetAll([FromQuery] BasePagingParams p)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DM_BieuMau_GetListPaging, EnumLogType.GetList, () =>
                {
                    int TotalRow = 0;
                    IList<DanhMucBieuMauModel> Data;
                    Data = _DanhMucBieuMauBUS.GetAll();
                    if (Data.Count == 0)
                    {
                        base.Status = 1;
                        base.Message = ConstantLogMessage.API_NoData;
                        return base.GetActionResult();
                    }
                    base.Status = Data.Count >= 0 ? 1 : 0;
                    base.Data = Data;

                    return base.GetActionResult();
                });
            }
            catch (Exception)
            {
                base.Status = -1;
                base.GetActionResult();
                throw;
            }
        }
    }
}
