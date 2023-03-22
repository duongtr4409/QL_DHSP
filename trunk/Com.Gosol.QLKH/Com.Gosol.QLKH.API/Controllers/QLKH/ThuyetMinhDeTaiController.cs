using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Gosol.QLKH.API.Authorization;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Security;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Com.Gosol.QLKH.API.Controllers.QLKH
{
    [Route("api/v1/ThuyetMinhDeTai")]
    [ApiController]
    public class ThuyetMinhDeTaiController : BaseApiController
    {
        private IThuyetMinhDeTaiBUS _ThuyetMinhDeTaiBUS;
        private IFileDinhKemBUS _FileDinhKemBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IHostingEnvironment _host;

        public ThuyetMinhDeTaiController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IOptions<AppSettings> Settings, IThuyetMinhDeTaiBUS ThuyetMinhDeTaiBUS, IFileDinhKemBUS FileDinhKemBUS, ILogHelper _LogHelper, ILogger<DeTaiController> logger) : base(_LogHelper, logger)
        {
            this._ThuyetMinhDeTaiBUS = ThuyetMinhDeTaiBUS;
            this._FileDinhKemBUS = FileDinhKemBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            this._host = hostingEnvironment;
        }

        [HttpGet]
        [Route("DanhSachDeXuatThuyetMinh")]
        public IActionResult DanhSachDeXuatThuyetMinh([FromQuery] BasePagingParams p, int CapQuanLy)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<DeXuatThuyetMinhModel> Data = _ThuyetMinhDeTaiBUS.GetPagingBySearch(p, CapQuanLy);
                    var clsCommon = new Commons();
                    foreach (var item in Data)
                    {
                        foreach (var thuyetminh in item.ListThuyetMinh)
                        {
                            if (thuyetminh.FileThuyetMinh.FileUrl != null && thuyetminh.FileThuyetMinh.FileUrl != "")
                            {
                                thuyetminh.FileThuyetMinh.FileUrl = clsCommon.GetServerPath(HttpContext) + thuyetminh.FileThuyetMinh.FileUrl;
                            }
                        }
                    }
                    base.Status = 1;
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

        [HttpPost]
        [Route("InsertThuyetMinh")]
        [CustomAuthAttribute(ChucNangEnum.thuyet_minh_de_tai, AccessLevel.Create)]
        public async Task<IActionResult> InsertThuyetMinh(IFormFile file, [FromForm] string ThuyetMinhDeTai)
        {
            if (file != null)
            {
                var ThuyetMinh = JsonConvert.DeserializeObject<ThuyetMinhDeTaiModel>(ThuyetMinhDeTai);
                var Result = _ThuyetMinhDeTaiBUS.InsertThuyetMinh(ThuyetMinh);
                int ThuyetMinhID = Utils.ConvertToInt32(Result.Data, 0);
                if (ThuyetMinhID > 0)
                {

                    var CanBoDangNhap = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                    var CoQuanDangNhap = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CoQuanID").Value, 0);
                    var clsCommon = new Commons();
                    var fileInfo = new FileDinhKemModel();
                    fileInfo.CoQuanID = CoQuanDangNhap;
                    fileInfo.NguoiTaoID = CanBoDangNhap;
                    fileInfo.LoaiFile = EnumLoaiFileDinhKem.ThuyetMinhDeTai.GetHashCode();
                    fileInfo.NghiepVuID = ThuyetMinhID;
                    fileInfo.FolderPath = "ThuyetMinhDeTai";
                    await clsCommon.InsertFileAsync(file, fileInfo, _host, _FileDinhKemBUS);
                    Result.Status = 1;
                    Result.Message = ConstantLogMessage.Alert_Insert_Success("thuyết minh");
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = ConstantLogMessage.Alert_Insert_Error("thuyết minh");
                }

                base.Status = Result.Status;
                base.Message = Result.Message;
                return base.GetActionResult();
            }
            else
            {
                base.Status = 0;
                base.Message = "Không có file thuyết minh";
                return base.GetActionResult();
            }
        }

        [HttpPost]
        [Route("UpdateThuyetMinh")]
        [CustomAuthAttribute(ChucNangEnum.thuyet_minh_de_tai, AccessLevel.Edit)]
        public async Task<IActionResult> UpdateThuyetMinh(IFormFile file, [FromForm] string ThuyetMinhDeTai)
        {
            var ThuyetMinh = JsonConvert.DeserializeObject<ThuyetMinhDeTaiModel>(ThuyetMinhDeTai);
            var data = _ThuyetMinhDeTaiBUS.UpdateThuyetMinh(ThuyetMinh);
            int res = Utils.ConvertToInt32(data.Data, 0);
            var Result = new BaseResultModel();
            if (res > 0)
            {
                if (file != null)
                {
                    //delete file
                    var ResultDeleteFile = _FileDinhKemBUS.Delete(EnumLoaiFileDinhKem.ThuyetMinhDeTai.GetHashCode(), ThuyetMinh.ThuyetMinhID);
                    //insert new file
                    if (ResultDeleteFile.Status > 0)
                    {
                        var clsCommon = new Commons();
                        var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                        FileDinhKemModel fileData = new FileDinhKemModel();
                        fileData.LoaiFile = EnumLoaiFileDinhKem.ThuyetMinhDeTai.GetHashCode();
                        fileData.NguoiTaoID = crCanBoID;
                        fileData.NghiepVuID = ThuyetMinh.ThuyetMinhID;
                        fileData.FolderPath = nameof(EnumLoaiFileDinhKem.ThuyetMinhDeTai);

                        var insertFile = await clsCommon.InsertFileAsync(file, fileData, _host, _FileDinhKemBUS);
                        if (insertFile)
                        {
                            Result.Status = 1;
                            Result.Message = Result.Message;
                        }
                        else
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể tải File";
                        }
                    }
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = ConstantLogMessage.Alert_Update_Success("thuyết minh");
                }
            }
            else
            {
                Result.Status = 0;
                Result.Message = data.Message;
            }
            base.Status = Result.Status;
            base.Message = Result.Message;
            return base.GetActionResult();
        }

        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetByID(int ThuyetMinhID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    ThuyetMinhDeTaiModel Data = _ThuyetMinhDeTaiBUS.GetByID(ThuyetMinhID);
                    var clsCommon = new Commons();
                    if (Data.FileThuyetMinh.FileUrl != null && Data.FileThuyetMinh.FileUrl != "")
                    {
                        Data.FileThuyetMinh.FileUrl = clsCommon.GetServerPath(HttpContext) + Data.FileThuyetMinh.FileUrl;
                    }
                    base.Status = 1;
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

        [HttpPost]
        [Route("Delete")]
        [CustomAuthAttribute(ChucNangEnum.thuyet_minh_de_tai, AccessLevel.Delete)]
        public IActionResult Delete([FromBody] int ThuyetMinhID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.DanhMuc_CapDeTai_Delete, EnumLogType.Delete, () =>
                {
                    var Result = _ThuyetMinhDeTaiBUS.Delete_ThuyetMinh(ThuyetMinhID);
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
        [Route("DanhSachAllThuyetMinhDeXuat")]
        public IActionResult DanhSachThuyetMinh(int DeXuatID)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    List<ThuyetMinhDeTaiModel> Data = _ThuyetMinhDeTaiBUS.GetAllThuyetMinhByDeXuatID(DeXuatID);
                    var clsCommon = new Commons();
                    foreach (var item in Data)
                    {
                        if (item.FileThuyetMinh.FileUrl != null && item.FileThuyetMinh.FileUrl != "")
                        {
                            item.FileThuyetMinh.FileUrl = clsCommon.GetServerPath(HttpContext) + item.FileThuyetMinh.FileUrl;
                        }
                    }
                    base.Status = 1;
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

        [HttpPost]
        [Route("DuyetThuyetMinh")]
        [CustomAuthAttribute(ChucNangEnum.thuyet_minh_de_tai, AccessLevel.Edit)]
        public IActionResult DuyetThuyetMinh([FromBody] ThuyetMinhDeTaiModel ThuyetMinhDeTaiModel)
        {
            try
            {
                return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                {
                    BaseResultModel Result = _ThuyetMinhDeTaiBUS.DuyetThuyetMinh(ThuyetMinhDeTaiModel.ThuyetMinhID, ThuyetMinhDeTaiModel.DeXuatID);
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