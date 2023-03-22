using System.IO;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Ums.Models.Common;
using Ums.Services.System;

namespace Ums.App.Base
{
    public class UploadController : Controller
    {
        private const string ExtsKeys = "ALLOW_FILE_EXTS";
        private const string MaxFileSize = "MAX_FILESIZE_UPLOAD";
        private readonly ISettingService _settingService;

        public UploadController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public object EditorUpload()
        {
            if (Request.Files.Count < 1) return null;
            var file = Request.Files[0];
            var fileexts = _settingService.GetValue(ExtsKeys).GetSplit('|').ToList();
            if (!fileexts.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                return $"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({Request.QueryString["CKEditorFuncNum"]}, '', 'File type is not allowed');</script>";
            }
            var maxsize = _settingService.GetValue(MaxFileSize).ToInt();
            if (file.ContentLength / (1024 * 1024) > maxsize)
            {
                return $"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({Request.QueryString["CKEditorFuncNum"]}, '', 'File is too large');</script>";
            }
            var folder = $"/Files/ContentUpload/{WorkContext.UserInfo.Id}/";
            var physicPath = Server.MapPath(folder);
            if (!Directory.Exists(physicPath)) Directory.CreateDirectory(physicPath);
            var filename = Common.GetUniqueFilename(file.FileName);
            file.SaveAs(physicPath + filename);
            var fileurl = folder + filename;
            return $"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({Request.QueryString["CKEditorFuncNum"]}, '{fileurl}', '');</script>";
        }

        public ActionResult JsUpload(string path)
        {
            if (Request.Files.Count < 1) return null;
            var file = Request.Files[0];
            var fileexts = _settingService.GetValue("UPLOAD_ALLOW_EXTS").GetSplit('|');
            if (!fileexts.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                return Json(file.FileName.ToResult(false, "File type is not allowed"));
            }
            var maxsize = _settingService.GetValue(MaxFileSize).ToInt();
            if (file.ContentLength / (1024 * 1024) > maxsize)
            {
                return Json(file.FileName.ToResult(false, "File is too large"));
            }
            var folder = $"/Files/{path}/{WorkContext.UserInfo.Id}/";
            var physicPath = Server.MapPath(folder);
            if (!Directory.Exists(physicPath)) Directory.CreateDirectory(physicPath);
            var filename = Common.GetUniqueFilename(file.FileName);
            file.SaveAs(physicPath + filename);
            return Json(new { file = folder + filename, name = file.FileName }.ToResult());
        }
    }
}