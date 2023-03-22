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
using Microsoft.Extensions.Caching.Memory;
using Com.Gosol.QLKH.Models.QLKH;

namespace Com.Gosol.QLKH.API.Controllers.QuanTriHeThong
{
    /*
     * api/v1/DataInCore
     */
    [Route("api/v1/DataInCore")]
    [ApiController]
    public class DataInCoreController : BaseApiController
    {
        private ISystemConfigBUS _SystemConfigBUS;
        private IOptions<AppSettings> _AppSettings;
        private RestShapAPIInCore rsApiInCore;
        private IMemoryCache _cache;
        public DataInCoreController(IMemoryCache memoryCache, IOptions<AppSettings> Settings, ISystemConfigBUS _SystemConfigBUS, ILogHelper _LogHelper, ILogger<DataInCoreController> logger) : base(_LogHelper, logger)
        {
            this._SystemConfigBUS = _SystemConfigBUS;
            _AppSettings = Settings;
            this.rsApiInCore = new RestShapAPIInCore(Settings, memoryCache);
            _cache = memoryCache;
        }



        #region Cán bộ

        /// <summary>
        /// API: LẤY DANH SÁCH CÁN BỘ THEO ĐƠN VỊ		
        /// </summary>
        /// <param name="departmentid"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getstave")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getstave(int departmentid, string keyword)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                // {
                if (keyword != null && keyword.Length > 0)
                    Data = rsApiInCore.core_DSCBTheoDonVi(departmentid).Where(x => x.TenCanBo.ToLower().Contains(keyword.ToLower())).ToList();
                else
                    Data = rsApiInCore.core_DSCBTheoDonVi(departmentid);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        /// <summary>
        /// API: LẤY DANH SÁCH CHỨC DANH		
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getTitles")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getTitles()
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                // {
                Data = rsApiInCore.core_getTitles();
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        /// <summary>
        /// API: LẤY DANH SÁCH CHỨC VỤ		
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getPositions")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getPositions()
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                // {
                Data = rsApiInCore.core_Positions();
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        /// <summary>
        /// API: LẤY DANH SÁCH HỌC HÀM HỌC VỊ		
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getDegrees")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getDegrees()
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                // {
                Data = rsApiInCore.core_getDegrees();
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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
        #endregion

        #region đơn vị
        /// <summary>
        /// lấy dánh đơn vị trực thuộc theo loại đơn vị,
        /// type =0 - lấy tất cả
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getDepartments")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getDepartments(int type)
        {
            try
            {
                //return CreateActionResult(ConstantLogMessage.HT_NhomNguoiDung_GetListPaging, EnumLogType.GetList, () =>
                // {
                Data = rsApiInCore.core_DSDonViTrucThuoc(type);
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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
        #endregion

        #region NCKH

        /// <summary>
        /// API: LẤY DANH SÁCH DANH MỤC NHIỆM VỤ NCKH		
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCategories")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getCategories(int? parentId)
        {
            try
            {
                //return CreateActionResult("get Categories", EnumLogType.GetList, () =>
                //{
                Data = rsApiInCore.core_Categories(parentId ?? 0);
                //var cacheEntry = _cache.Get<List<CategoriesModel>>(CacheKeys.Categories);
                //if (cacheEntry == null || cacheEntry.Count < 1)
                //{
                //    cacheEntry = _cache.GetOrCreate(CacheKeys.Categories, entry =>
                //    {
                //        entry.SlidingExpiration = TimeSpan.FromDays(1);
                //        return rsApiInCore.core_Categories(parentId ?? 0);
                //    });
                //}
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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


        /// <summary>
        /// lấy danh sách categories từ code và ghép cây cha - con
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getCategories_relation")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getCategories_relation()
        {
            try
            {
                Data = rsApiInCore.core_Categories_relation();
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


        /// <summary>
        /// API: LẤY all DANH SÁCH DANH MỤC NHIỆM VỤ NCKH		
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getALLCategories")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getALLCategories()
        {
            try
            {
                //return CreateActionResult("get all Categories", EnumLogType.GetList, () =>
                //{
                Data = rsApiInCore.core_GetALLCategories();
                base.Status = 1;
                base.TotalRow = TotalRow;
                base.Data = Data;
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

        /// <summary>
        /// ds categories != 34 (systemconfig - CATEGORY_DETAI) , và conversion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DSNhiemVuKhoaHoc")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult DSNhiemVuKhoaHoc()
        {
            try
            {
                return CreateActionResult("ds nhiệm vụ khoa học", EnumLogType.GetList, () =>
                {
                    Data = rsApiInCore.core_DSNhiemVuKhoaHoc();
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


        /// <summary>
        /// API: LẤY DANH SÁCH QUY ĐỔI NHIỆM VỤ NCKH		
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getConversions")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getConversions(int? categoryId)
        {
            try
            {
                return CreateActionResult("get Conversions", EnumLogType.GetList, () =>
                {
                    Data = rsApiInCore.core_getConversions(categoryId ?? 0);
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

        /// <summary>
        /// API: LẤY DANH SÁCH NHIỆM VỤ NCKH CỦA CÁN BỘ		
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="yearId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getTasks")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getTasks(int staffId, int yearId)
        {
            try
            {
                return CreateActionResult("getTasks", EnumLogType.GetList, () =>
                {
                    Data = rsApiInCore.core_getTasks(staffId, yearId);
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
        #endregion


        #region đào tạo

        /// <summary>
        /// API: LẤY DANH SÁCH NĂM HỌC		
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getYears")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult getYears()
        {
            try
            {
                return CreateActionResult("getYears", EnumLogType.GetList, () =>
                {
                    Data = rsApiInCore.core_getYears();
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
        #endregion



        [HttpGet]
        [Route("clearCache")]
        //[CustomAuthAttribute(ChucNangEnum.HeThong_QuanLy_DataInCore, AccessLevel.Read)]
        public IActionResult clearCache()
        {
            try
            {
                return CreateActionResult("clearCache", EnumLogType.GetList, () =>
                {
                    Data = rsApiInCore.clearCache();
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