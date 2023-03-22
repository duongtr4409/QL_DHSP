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
using Com.Gosol.QLKH.Models.QLKH;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    [Route("api/v1/AttachFile")]
    [ApiController]
    public class AttachFileController : BaseApiController
    {
        private IChucNangBUS _ChucNangBUS;
        private IHostingEnvironment _host;
        public AttachFileController(IHostingEnvironment hostingEnvironment, IChucNangBUS ChucNangBUS, ILogHelper _LogHelper, ILogger<AttachFileController> logger) : base(_LogHelper, logger)
        {
            this._ChucNangBUS = ChucNangBUS;
            this._host = hostingEnvironment;
        }

        [HttpPost]
        //[CustomAuthAttribute(ChucNangEnum.KeKhai_FileDinhKem, AccessLevel.Create)]
        [Route("UpLoadFile")]
        public IActionResult UpLoadFile()
        {
            try
            {
                //var crCanBoID = Utils.ConvertToInt32(User.Claims.FirstOrDefault(c => c.Type == "CanBoID").Value, 0);
                var FileDinhKem = Request.Form.Files;
                return CreateActionResult("UpLoad File", EnumLogType.Insert, () =>
                {
                    if (FileDinhKem.Count > 0)
                    {
                        for (int i = 0; i < FileDinhKem.Count; i++)
                        {
                            var crObj = new FileDinhKemModel();
                            //crObj.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + FileDinhKem[i].FileName;
                            crObj.TenFileHeThong = DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + FileDinhKem[i].FileName;
                            crObj.TenFileGoc = FileDinhKem[i].FileName;

                        }
                        base.Status = 1;
                        base.Message = "Thêm mới file đính kèm thành công";
                    }
                    else
                    {
                        base.Status = 0;
                        base.Message = "Vui lòng chọn file đính kèm";
                    }
                    return base.GetActionResult();
                });
            }
            catch (Exception ex)
            {
                base.Status = -1;
                base.GetActionResult();
                throw ex;
            }
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IList<IFormFile> files, [FromForm] string ThongTinCanBo)
        {
            var result = JsonConvert.DeserializeObject<CanBoModel>(ThongTinCanBo);
            // thêm bảng nghiệp vụ
            var clsCommon = new Commons();
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                // thêm bảng file đính kèm
                filename = clsCommon.EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(clsCommon.GetSavePathFile(_host, filename)))
                    await source.CopyToAsync(output);
            }

            return base.GetActionResult();
        }

        [HttpPost]
        [Route("Upload1")]
        //[RequestSizeLimit(2147483647)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, MultipartHeadersCountLimit = int.MaxValue, MultipartHeadersLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Upload1(IList<IFormFile> files, [FromForm] string ThongTinCanBo)
        {
            var result = JsonConvert.DeserializeObject<CanBoModel>(ThongTinCanBo);
            // thêm bảng nghiệp vụ
            var clsCommon = new Commons();
            foreach (IFormFile source in files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                // thêm bảng file đính kèm
                filename = clsCommon.EnsureCorrectFilename(filename);
                FileDinhKemModel file = new FileDinhKemModel();
                file.NghiepVuID = 1;
                file.LoaiFile = EnumLoaiFileDinhKem.AnhDaiDien.GetHashCode();
                file.FolderPath = nameof(EnumLoaiFileDinhKem.AnhDaiDien);
                file.NguoiTaoID = CanBoID;
                file.CoQuanID = CoQuanID;

                //using (FileStream output = System.IO.File.Create(clsCommon.GetSavePathFile(_host, filename)))
                await clsCommon.InsertFileAsync(source, file, _host);
            }

            return base.GetActionResult();
        }



        //private string EnsureCorrectFilename(string filename)
        //{
        //    if (filename.Contains("\\"))
        //        filename = filename.Substring(filename.LastIndexOf("\\") + 1);

        //    return filename;
        //}

        //private string GetPathAndFilename(string filename)
        //{
        //    return _host.ContentRootPath + "\\Upload\\CanBo\\" + filename;
        //}

        //[HttpPost]
        //public async Task<IActionResult> Upload(IList<IFormFile> files)
        //{
        //    foreach (var file in files)
        //    {
        //        var fileName = ContentDispositionHeaderValue
        //            .Parse(file.ContentDisposition)
        //            .FileName
        //            .Trim('"');

        //        var filePath = _hostingEnvironment.WebRootPath + "\\wwwroot\\" + fileName;
        //        await file.SaveAsAsync(filePath);
        //    }
        //    return View();
        //}
    }

}