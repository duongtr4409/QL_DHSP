
using Castle.Core.Internal;
using Com.Gosol.QLKH.API.Config;
using Com.Gosol.QLKH.API.Formats;
using Com.Gosol.QLKH.BUS.QLKH;
using Com.Gosol.QLKH.DAL.QuanTriHeThong;
using Com.Gosol.QLKH.Models;
using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using Com.Gosol.QLKH.Models.QuanTriHeThong;
using Com.Gosol.QLKH.Ultilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509.Qualified;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.API.Controllers
{
    public class Commons
    {
        /// <summary>
        /// save base64 to file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="TenFileHeThong"></param>
        /// <returns></returns>
        public bool SaveBase64ToFile(FileModel file, string TenFileHeThong, string pathSaveFile)
        {
            try
            {
                if (file.Base64.Length > 0)
                {
                    var b64 = file.Base64;
                    b64 = b64.Split(',')[1];
                    byte[] bytes = Convert.FromBase64String(b64);
                    //var folderName = Path.Combine("Upload", "FileDinhKemDuyetKeKhai");
                    var folderName = Path.Combine(pathSaveFile);
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, TenFileHeThong);
                    System.IO.File.WriteAllBytes(fullPath, bytes);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public string ConvertFileToBase64(string pathFile)
        {
            try
            {
                var at = System.IO.File.GetAttributes(pathFile);

                byte[] fileBit = System.IO.File.ReadAllBytes(pathFile);
                var file = System.IO.Path.Combine(pathFile);

                string AsBase64String = Convert.ToBase64String(fileBit);
                return AsBase64String;
            }
            catch (Exception ex)
            {
                return string.Empty;
                throw ex;
            }
        }

        public string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public string GetSavePathFile(IHostingEnvironment _host, string filename, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            return _host.ContentRootPath + "\\" + sysCF.GetByKey("SavePathFile").ConfigValue + "\\" + folderPath + "\\" + filename;
        }

        public void CheckAndCreateFolder(IHostingEnvironment _host, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            string path = _host.ContentRootPath + "\\" + sysCF.GetByKey("SavePathFile").ConfigValue + "\\" + folderPath;
            bool isFolder = Directory.Exists(path);
            if (!isFolder)
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetUrlFile(string filename, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            var pathfile = sysCF.GetByKey("SavePathFile").ConfigValue;

            return pathfile + "\\" + folderPath + "\\" + filename;
        }
        public string GetServerPath(HttpContext httpCT)
        {
            return httpCT.Request.Scheme + "://" + httpCT.Request.Host.Value + "\\";
        }
        public FileDinhKemModel GetInfoFileDinhKem(IFormFile FileDinhKem, int CanBoID, IHostingEnvironment _host)
        {
            FileDinhKemModel fileInfo = new FileDinhKemModel();
            var crCanBoID = CanBoID;
            fileInfo.TenFileGoc = ContentDispositionHeaderValue.Parse(FileDinhKem.ContentDisposition).FileName.Trim('"');
            fileInfo.TenFileGoc = EnsureCorrectFilename(fileInfo.TenFileGoc);
            fileInfo.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + fileInfo.TenFileGoc;
            fileInfo.NguoiTaoID = crCanBoID;
            fileInfo.LoaiFile = EnumLoaiFileDinhKem.DanhMucBieuMau.GetHashCode();
            fileInfo.NgayTao = DateTime.Now;
            fileInfo.FileUrl = GetSavePathFile(_host, fileInfo.TenFileHeThong);
            return fileInfo;
        }

        public async Task<bool> InsertFileAsync(IFormFile source, FileDinhKemModel FileDinhKem, IHostingEnvironment _host, IFileDinhKemBUS _FileDinhKemBUS)
        {
            try
            {
                FileDinhKemModel fileInfo = new FileDinhKemModel();
                var crCanBoID = FileDinhKem.NguoiTaoID;
                fileInfo.NghiepVuID = FileDinhKem.NghiepVuID;
                fileInfo.CoQuanID = FileDinhKem.CoQuanID;
                fileInfo.TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                fileInfo.TenFileGoc = EnsureCorrectFilename(fileInfo.TenFileGoc);
                fileInfo.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + fileInfo.TenFileGoc;
                fileInfo.NguoiTaoID = crCanBoID;
                fileInfo.LoaiFile = FileDinhKem.LoaiFile;
                fileInfo.NgayTao = DateTime.Now;
                fileInfo.FileUrl = GetUrlFile(fileInfo.TenFileHeThong, FileDinhKem.FolderPath);
                fileInfo.NoiDung = FileDinhKem.NoiDung;
                var ResultFile = _FileDinhKemBUS.Insert(fileInfo);
                if (ResultFile.Status > 0)
                {
                    //Add file vào thư mục server
                    try
                    {
                        CheckAndCreateFolder(_host, FileDinhKem.FolderPath);
                        using (FileStream output = File.Create(GetSavePathFile(_host, fileInfo.TenFileHeThong, FileDinhKem.FolderPath)))
                            await source.CopyToAsync(output);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }
        public async Task<bool> InsertFileAsync(IFormFile source, FileDinhKemModel FileDinhKem, IHostingEnvironment _host)
        {
            try
            {
                FileDinhKemModel fileInfo = new FileDinhKemModel();
                var crCanBoID = FileDinhKem.NguoiTaoID;
                fileInfo.NghiepVuID = FileDinhKem.NghiepVuID;
                fileInfo.CoQuanID = FileDinhKem.CoQuanID;
                fileInfo.TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                fileInfo.TenFileGoc = EnsureCorrectFilename(fileInfo.TenFileGoc);
                fileInfo.TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + fileInfo.TenFileGoc;
                fileInfo.NguoiTaoID = crCanBoID;
                fileInfo.LoaiFile = FileDinhKem.LoaiFile;
                fileInfo.NgayTao = DateTime.Now;
                fileInfo.FileUrl = GetUrlFile(fileInfo.TenFileHeThong, FileDinhKem.FolderPath);
                fileInfo.NoiDung = FileDinhKem.NoiDung;
                //var ResultFile = _FileDinhKemBUS.Insert(fileInfo);
                //if (ResultFile.Status > 0)
                //{
                //Add file vào thư mục server
                try
                {
                    CheckAndCreateFolder(_host, FileDinhKem.FolderPath);
                    using (FileStream output = File.Create(GetSavePathFile(_host, fileInfo.TenFileHeThong, FileDinhKem.FolderPath)))
                        await source.CopyToAsync(output);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }
    }
    public class FileModel
    {
        public string TenFile { get; set; }
        public string Base64 { get; set; }
    }

    public class RestShapAPIInCore
    {
        public IOptions<AppSettings> settings;
        private IMemoryCache _cache;
        private SystemConfigDAL syscf;
        public RestShapAPIInCore(IOptions<AppSettings> Settings)
        {
            this.settings = Settings;
            this.syscf = new SystemConfigDAL();
        }
        public RestShapAPIInCore(IOptions<AppSettings> Settings, IMemoryCache memoryCache)
        {
            this.settings = Settings;
            _cache = memoryCache;
            this.syscf = new SystemConfigDAL();
        }

        #region người dùng
        public NguoiDungCoreModel core_Login(string userName, string passWord)
        {
            var result = new NguoiDungCoreModel();
            try
            {
                var client = new RestClient(settings.Value.api_validate + "?username=" + userName + "&password=" + passWord);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("token", settings.Value.token);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (rs.success == true)
                    {
                        result = JsonConvert.DeserializeObject<NguoiDungCoreModel>(rs.data.ToString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        public NguoiDungCoreModel core_LoginSSO(string accessToken)
        {
            var result = new NguoiDungCoreModel();
            try
            {
                var client = new RestClient(settings.Value.api_getuser_sso + accessToken);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("token", settings.Value.apptoken);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (rs.success == true)
                    {
                        result = JsonConvert.DeserializeObject<NguoiDungCoreModel>(rs.data.ToString());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        public NguoiDungCoreModel DuowngToraFake_Core_LoginSSO(string accessToken)
        {
            var result = new NguoiDungCoreModel();
            try
            {
                result.Id = 8888;
               // 4177 laf ADMIN xem trong DB
               if(accessToken.IsNullOrEmpty() || accessToken.Equals("?ADMIN"))
                {
                    result.UserKey = "4177";
                    result.Username = "Admin-DuowngTora";
                    result.Name = "Admin DuowngTora";
                }else if(accessToken.Equals("?QLKH"))
                {
                    result.UserKey = "60";
                    result.Username = "QLKH-DuowngTora";
                    result.Name = "QLKH DuowngTora";
                    
                }else if (accessToken.Equals("?NKH"))
                {
                    result.UserKey = "58";
                    result.Username = "NKH-DuowngTora";
                    result.Name = "NKH DuowngTora";
                }
                result.StaffId = 1;
                
                var client = new RestClient(settings.Value.api_getuser_sso + accessToken);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                
                /* OLD

                //var client = new RestClient(settings.Value.api_getuser_sso + accessToken);
                //client.Timeout = -1;
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("token", settings.Value.apptoken);
                //IRestResponse response = client.Execute(request);
                //if (response.IsSuccessful == true)
                //{
                //    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                //    if (rs.success == true)
                //    {
                //        result = JsonConvert.DeserializeObject<NguoiDungCoreModel>(rs.data.ToString());
                //    }
                //}
                */
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        public bool core_changepassword(int userId, string passWord)
        {
            var result = false;
            try
            {
                var client = new RestClient(settings.Value.api_changepassword + "?userId=" + userId + "&newpassword=" + passWord);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("token", settings.Value.token);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (Utils.ConvertToBoolean(rs.data, false))
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }



        /// <summary>
        /// Lấy danh sách người dùng theo đơn vị - core model
        /// </summary>
        /// <param name="donViID"></param>
        /// <returns></returns>
        public List<CoreDataModel> core_getusers(int donViID)
        {
            var result = new List<CoreDataModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CoreDataModel>>(CacheKeys.UserInCore);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.UserInCore, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getusers + 0 + "&PageNumber=1&PageSize=999999999");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                                }
                                else return new List<CoreDataModel>();
                            }
                            else return new List<CoreDataModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getusers + donViID + "&PageNumber=1&PageSize=999999999");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString());
                        }
                    }
                }
                return result.Where(x => x.DepartmentId == donViID || donViID == 0).ToList(); ;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        public List<CoreDataModel> DuowngToraFake_Core_getusers(int donViID)
        {
            var result = new List<CoreDataModel>();
            //try
            //{
            //    if (_cache != null)
            //    {
            //        result = _cache.Get<List<CoreDataModel>>(CacheKeys.UserInCore);
            //        if (result == null || result.Count < 1)
            //        {
            //            result = _cache.GetOrCreate(CacheKeys.UserInCore, entry =>
            //            {
            //                entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
            //                var client = new RestClient(settings.Value.api_getusers + 0 + "&PageNumber=1&PageSize=999999999");
            //                client.Timeout = -1;
            //                var request = new RestRequest(Method.GET);
            //                request.AddHeader("token", settings.Value.token);
            //                IRestResponse response = client.Execute(request);
            //                if (response.IsSuccessful == true)
            //                {
            //                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
            //                    if (rs.success == true)
            //                    {
            //                        return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
            //                    }
            //                    else return new List<CoreDataModel>();
            //                }
            //                else return new List<CoreDataModel>();
            //            });
            //        }
            //    }
            //    else
            //    {
            //        var client = new RestClient(settings.Value.api_getusers + donViID + "&PageNumber=1&PageSize=999999999");
            //        client.Timeout = -1;
            //        var request = new RestRequest(Method.GET);
            //        request.AddHeader("token", settings.Value.token);
            //        IRestResponse response = client.Execute(request);
            //        if (response.IsSuccessful == true)
            //        {
            //            var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
            //            if (rs.success == true)
            //            {
            //                result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString());
            //            }
            //        }
            //    }
                return result.Where(x => x.DepartmentId == donViID || donViID == 0).ToList();
            //}
            //catch (Exception ex)
            //{
            //    //this.LogError("core_Login - " + ex);
            //    return result;
            //    throw ex;
            //}

        }

        /// <summary>
        /// DS Người dùng theo cơ quan
        /// </summary>
        /// <param name="donViID"></param>
        /// <returns></returns>
        public List<HeThongNguoiDungModel> core_DSNguoiDungTheoCoQuan(int donViID)
        {
            var result = new List<HeThongNguoiDungModel>();
            try
            {
                List<CoreDataModel> listInCore = core_getusers(donViID);
                foreach (var item in listInCore)
                {
                    HeThongNguoiDungModel heThongNguoiDung = new HeThongNguoiDungModel();
                    heThongNguoiDung.TenNguoiDung = item.Username;
                    heThongNguoiDung.NguoiDungID = item.Id;
                    heThongNguoiDung.CoQuanID = item.DepartmentId;
                    heThongNguoiDung.CanBoID = item.StaffId;
                    heThongNguoiDung.TenCanBo = item.Name;
                    result.Add(heThongNguoiDung);
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }


        /// <summary>
        /// Change password 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        //public bool core_changepassword(int userId, string newpassword)
        //{
        //    var result = false;
        //    try
        //    {
        //        var client = new RestClient(settings.Value.api_changepassword + "?userId=" + userId + "&newpassword=" + newpassword);
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("token", settings.Value.token);
        //        IRestResponse response = client.Execute(request);
        //        if (response.IsSuccessful == true)
        //        {
        //            var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
        //            if (rs.success == true)
        //                result = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //    return result;
        //}
        #endregion

        #region cán bộ
        /// <summary>
        /// API: LẤY DANH SÁCH CÁN BỘ THEO ĐƠN VỊ - model core
        /// </summary>
        /// <param name="donViID"></param>
        /// <returns></returns>
        public List<CoreDataModel> core_DSCanBoTheoDonVi(int donViID)
        {
            var result = new List<CoreDataModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CoreDataModel>>(CacheKeys.StaffInCore);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.StaffInCore, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getstave + 0 + "&PageNumber=1&PageSize=999999999");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                                }
                                else return new List<CoreDataModel>();
                            }
                            else return new List<CoreDataModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getstave + donViID + "&PageNumber=1&PageSize=999999999");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                        }
                    }
                }
                return result.Where(x => x.DepartmentId == donViID || donViID == 0).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// API: LẤY THÔNG TIN CÁN BỘ - model core
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CoreDataModel core_getstaff(int id)
        {
            var result = new CoreDataModel();
            try
            {
                var client = new RestClient(settings.Value.api_getstaff + id);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("token", settings.Value.token);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (rs.success == true)
                    {
                        result = JsonConvert.DeserializeObject<CoreDataModel>(rs.data.ToString()) ?? new CoreDataModel();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        /// <summary>
        /// API: LẤY DANH SÁCH CÁN BỘ THEO ĐƠN VỊ - model phần mềm		
        /// </summary>
        /// <param name="donViID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> core_DSCBTheoDonVi(int donViID)
        {
            var result = new List<HeThongCanBoModel>();
            try
            {
                var listInCore = core_DSCanBoTheoDonVi(donViID);
                foreach (var item in listInCore)
                {
                    var i = new HeThongCanBoModel();
                    i.CanBoID = item.Id;
                    i.TenCanBo = item.Name;
                    i.GioiTinhStr = item.Gender;
                    i.CoQuanID = item.DepartmentId;
                    i.MaCB = item.Code;
                    i.ChucDanhID = item.TitleId;
                    i.FName = item.FName;
                    i.HocHamHocViID = item.DegreeId;
                    i.DanhSachChucVuID = new List<int>() { item.TitleId };
                    i.Email = item.Email;
                    i.NgaySinh = item.Birthday;
                    result.Add(i);
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        /// <summary>
        /// API: LẤY DANH SÁCH CHỨC DANH - model core
        /// </summary>
        /// <returns></returns>
        public List<CoreDataModel> core_getTitles()
        {
            var result = new List<CoreDataModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CoreDataModel>>(CacheKeys.Titles);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Titles, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getTitles);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                                }
                                else return new List<CoreDataModel>();
                            }
                            else return new List<CoreDataModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getTitles);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }

        /// <summary>
        /// API: LẤY DANH SÁCH CHỨC VỤ		
        /// </summary>
        /// <returns></returns>
        public List<CoreDataModel> core_Positions()
        {
            var result = new List<CoreDataModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CoreDataModel>>(CacheKeys.Positions);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Positions, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getPositions);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                                }
                                else return new List<CoreDataModel>();
                            }
                            else return new List<CoreDataModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getPositions);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// API: LẤY DANH SÁCH HỌC HÀM HỌC VỊ - model core
        /// </summary>
        /// <returns></returns>
        public List<CoreDataModel> core_getDegrees()
        {
            var result = new List<CoreDataModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CoreDataModel>>(CacheKeys.Degrees);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Degrees, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getDegrees);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                                }
                                else return new List<CoreDataModel>();
                            }
                            else return new List<CoreDataModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getDegrees);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CoreDataModel>>(rs.data.ToString()) ?? new List<CoreDataModel>();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }


        #endregion

        #region đơn vị

        /// <summary>
        /// lấy dánh đơn vị trực thuộc theo loại đơn vị,
        /// type =0 - lấy tất cả
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DonViCoreModel> core_DSDonViTrucThuoc(int type)
        {
            var result = new List<DonViCoreModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<DonViCoreModel>>(CacheKeys.Departments);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Departments, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getDepartments + 0);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<DonViCoreModel>>(rs.data.ToString()) ?? new List<DonViCoreModel>();
                                }
                                else return new List<DonViCoreModel>();
                            }
                            else return new List<DonViCoreModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getDepartments + type);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<DonViCoreModel>>(rs.data.ToString()) ?? new List<DonViCoreModel>();
                        }
                    }
                }
                return result.Where(x => x.type == type || type == 0).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }

        }
        #endregion
        public List<DonViCanBoModel> GetALLCanBo()
        {
            var result = new List<DonViCanBoModel>();
            try
            {
                var listDonVi = core_DSDonViTrucThuoc(0);
                foreach (var item in listDonVi)
                {
                    DonViCanBoModel dv = new DonViCanBoModel();
                    dv.Id = item.Id;
                    dv.Name = item.Name;
                    dv.CoQuanID = item.Id;
                    result.Add(dv);
                }
                var listCanBo = core_DSCanBoTheoDonVi(0);
                foreach (var item in listCanBo)
                {
                    DonViCanBoModel cb = new DonViCanBoModel();
                    cb.Id = item.Id;
                    cb.Name = item.Name;
                    cb.DepartmentId = item.DepartmentId;
                    result.Add(cb);
                }
                result.OrderBy(x => x.Id).ToList();
                result.ForEach(x => x.Children = result.Where(i => i.DepartmentId == x.CoQuanID && i.DepartmentId > 0 && x.CoQuanID > 0).ToList());
                result.RemoveAll(x => x.DepartmentId > 0);

                List<DonViCanBoModel> data = new List<DonViCanBoModel>();
                DonViCanBoModel toantruong = new DonViCanBoModel();
                toantruong.Id = 0;
                toantruong.Name = "Trường Đại học Sư phạm Hà Nội";
                toantruong.Children = result;
                data.Add(toantruong);
                return data;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }

        #region NCKH

        /// <summary>
        /// API: LẤY DANH SÁCH DANH MỤC NHIỆM VỤ NCKH
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<CategoriesModel> core_Categories(int parentId)
        {
            var result = new List<CategoriesModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CategoriesModel>>(CacheKeys.Categories);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Categories, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getCategories + "-1");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                                }
                                else return new List<CategoriesModel>();
                            }
                            else return new List<CategoriesModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getCategories + parentId);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                        }
                    }
                }
                return result.Where(x => x.ParentId == parentId || parentId == -1).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        public List<CategoriesModel> core_Categories_relation()
        {
            var result = new List<CategoriesModel>();
            try
            {
                var lstCategories = new List<CategoriesModel>();
                lstCategories = core_Categories(-1);
                lstCategories.ForEach(x => result.Add((CategoriesModel)x.Shallowcopy()));
                result.OrderBy(x => x.ParentId).ToList();
                result.ForEach(x => x.Children = result.Where(i => i.ParentId == x.Id).ToList());
                result.RemoveAll(x => x.ParentId > 0);
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }
        public List<CategoriesModel> core_Categories1(int parentId)
        {
            var result = new List<CategoriesModel>();
            try
            {
                if (CacheKeys.lstCategories != null && CacheKeys.lstCategories.Count > 0)
                {
                    result = CacheKeys.lstCategories;
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getCategories + "-1");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                        }
                    }
                    CacheKeys.lstCategories = result;
                }
                return result.Where(x => x.ParentId == parentId || parentId == -1).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// ghép cả categories và conversion
        /// </summary>
        /// <returns></returns>
        public List<CategoriesModel> core_GetALLCategories()
        {
            List<CategoriesModel> lstCategories = new List<CategoriesModel>();
            List<CategoriesModel> result = new List<CategoriesModel>();

            try
            {

                lstCategories = core_Categories(-1);
                List<CategoriesModel> lstConversion = new List<CategoriesModel>();
                lstConversion = core_getConversions(0);
                lstConversion.ForEach(x => x.ParentId = x.CategoryId);
                lstCategories.ForEach(x => result.Add((CategoriesModel)x.Shallowcopy()));
                result.AddRange(lstConversion);
                result.OrderBy(x => x.ParentId).ToList();
                result.ForEach(x => x.Children = result.Where(i => i.ParentId == x.Id).ToList());
                result.RemoveAll(x => x.ParentId > 0);
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }
        /// <summary>
        /// ds categories != 34 (systemconfig - CATEGORY_DETAI) , và conversion
        /// </summary>
        /// <returns></returns>
        public List<CategoriesModel> core_DSNhiemVuKhoaHoc()
        {
            List<CategoriesModel> lstCategories = new List<CategoriesModel>();
            List<CategoriesModel> result = new List<CategoriesModel>();
            try
            {
                var scf = new SystemConfigDAL();
                var categoryDeTaiID = Utils.ConvertToInt32(scf.GetByKey("CATEGORY_DETAI").ConfigValue, 34);
                lstCategories = core_Categories(-1);
                List<CategoriesModel> lstConversion = new List<CategoriesModel>();
                lstConversion = core_getConversions(0);
                lstConversion.ForEach(x => x.ParentId = x.CategoryId);
                lstCategories.ForEach(x => result.Add((CategoriesModel)x.Shallowcopy()));
                result = result.Where(x => x.Id != categoryDeTaiID && x.ParentId != categoryDeTaiID).ToList();
                result.OrderBy(x => x.ParentId).ToList();
                result.ForEach(x => x.Children = lstConversion.Where(i => i.CategoryId == x.Id).ToList());
                result.ForEach(x => x.Children = ((x.Children != null && x.Children.Count > 0) ? x.Children : result.Where(i => i.ParentId == x.Id).ToList()));

                result.RemoveAll(x => x.ParentId > 0);
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// API: LẤY DANH SÁCH QUY ĐỔI NHIỆM VỤ NCKH theo categoryid
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<CategoriesModel> core_getConversions(int categoryId)
        {
            var result = new List<CategoriesModel>();
            try
            {
                if (_cache != null)
                {
                    result = _cache.Get<List<CategoriesModel>>(CacheKeys.Conversions);
                    if (result == null || result.Count < 1)
                    {
                        result = _cache.GetOrCreate(CacheKeys.Conversions, entry =>
                        {
                            entry.SlidingExpiration = TimeSpan.FromDays(Utils.ConvertToInt32(syscf.GetByKey("CACHE_EXPIRATION").ConfigValue, 30));
                            var client = new RestClient(settings.Value.api_getConversions + 0);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.GET);
                            request.AddHeader("token", settings.Value.token);
                            IRestResponse response = client.Execute(request);
                            if (response.IsSuccessful == true)
                            {
                                var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                                if (rs.success == true)
                                {
                                    return JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                                }
                                else return new List<CategoriesModel>();
                            }
                            else return new List<CategoriesModel>();
                        });
                    }
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getConversions + categoryId);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                        }
                    }
                }
                return result.Where(x => x.CategoryId == categoryId || categoryId == 0).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        public List<CategoriesModel> core_getConversions1(int categoryId)
        {
            var result = new List<CategoriesModel>();
            try
            {
                if (CacheKeys.lstConversions != null && CacheKeys.lstConversions.Count > 0)
                {
                    result = CacheKeys.lstConversions;
                }
                else
                {
                    var client = new RestClient(settings.Value.api_getConversions + 0);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("token", settings.Value.token);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful == true)
                    {
                        var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                        if (rs.success == true)
                        {
                            result = JsonConvert.DeserializeObject<List<CategoriesModel>>(rs.data.ToString()) ?? new List<CategoriesModel>();
                        }
                    }
                    CacheKeys.lstConversions = result;
                }
                return result.Where(x => x.CategoryId == categoryId || categoryId == 0).ToList();
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }


        /// <summary>
        /// API: LẤY DANH SÁCH NHIỆM VỤ NCKH CỦA CÁN BỘ		
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public List<Tasks> core_getTasks(int staffId, int yearId)
        {
            var result = new List<Tasks>();
            try
            {
                var client = new RestClient(settings.Value.api_getTasks + "?staffId=" + staffId + "&yearId=" + yearId);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("token", settings.Value.token);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (rs.success == true)
                    {
                        result = JsonConvert.DeserializeObject<List<Tasks>>(rs.data.ToString()) ?? new List<Tasks>();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }



        /// <summary>
        /// API: LẤY DANH SÁCH NĂM HỌC		
        /// </summary>
        /// <returns></returns>
        public List<Year> core_getYears()
        {
            var result = new List<Year>();
            try
            {
                var client = new RestClient(settings.Value.api_getYears);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("token", settings.Value.token);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful == true)
                {
                    var rs = JsonConvert.DeserializeObject<CoreResposeBase>(response.Content);
                    if (rs.success == true)
                    {
                        result = JsonConvert.DeserializeObject<List<Year>>(rs.data.ToString()) ?? new List<Year>();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //this.LogError("core_Login - " + ex);
                return result;
                throw ex;
            }
        }

        #endregion

        public bool clearCache()
        {
            var result = true;
            try
            {
                _cache.Remove(CacheKeys.Categories);
                _cache.Remove(CacheKeys.Conversions);
                _cache.Remove(CacheKeys.Degrees);
                _cache.Remove(CacheKeys.Departments);
                _cache.Remove(CacheKeys.Positions);
                _cache.Remove(CacheKeys.StaffInCore);
                _cache.Remove(CacheKeys.Titles);
                _cache.Remove(CacheKeys.UserInCore);

                return result;
            }
            catch (Exception)
            {
                return result;
                throw;
            }
        }
    }
}
