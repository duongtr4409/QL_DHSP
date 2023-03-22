using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoreAPI.Entity;
using System.Net.Http.Headers;
using System.Net;

namespace CoreAPI
{
    public static class CoreAPI
    {
        //public const string BASE_URL_API = "https://ums.hnue.edu.vn";
        public const string BASE_URL_API = "https://ums.2bsystem.com.vn";
        public const string BASE_URL = "https://qlnt.hnue.edu.vn"; // base url lay data trả lại cho bên core


        public const string appToken = "GWtfLA550PXjEhnakdllmP1gJwLKiHwqvqSCr4vli4FU3vdW9rW6HGj1eMWYyvXtdrxYOksqr9DrJhmXTzK1v0jpHbiehWxxhW0M7FHY8qOHC1r6k8F8kf7nIHRxBOIV637413981455256312";
        public const string home = "http://localhost:50498/";
        //public const string home = "http://14.225.5.64:8765/";
        //public const string home = "http://qlncs.hnue.edu.vn/";
        public const string ApiBase = "https://id.2bsystem.com.vn";
        public const string token_auth = "vYlFowfUqJ2jc2BVCas0QixbBa1uK4ehekwn7cpdAsmiYPVcFOXKzBuMlJEO5uKvoVF1FXYjuP0i6ODvEZp5hUsczjCyXbiPh04CKdWr0xakLLPWFTJfFPOUmmCgVjMS637409529212396028";
        /// <summary>
        /// field that holds your api token
        /// </summary>
        private static string _AuthToken = "";

        /// <summary>
        /// This property sets you Api token.
        /// </summary>
        public static string AuthToken
        {
            get { return _AuthToken; }
            set { _AuthToken = value; }
        }

        /// <summary>
        /// Repressents if the token is set.
        /// </summary>
        private static bool TokenIsSet => AuthToken.Any();


        /// <summary>
        /// DS cac khoa dao tao
        /// </summary>
        /// <returns></returns>
        public static async Task<List<OrganizationInfo>> GetListKhoa()
        {
            List<OrganizationInfo> list = new List<OrganizationInfo>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/organize/getfaculties";
                    client.DefaultRequestHeaders.Add("Token", token_auth);
                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<OrganizationInfo>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        /// <summary>
        /// DS cac phong ban
        /// </summary>
        /// <returns></returns>
        public static async Task<List<OrganizationInfo>> GetListDivisions()
        {
            List<OrganizationInfo> list = new List<OrganizationInfo>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/organize/getdivisions	";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<OrganizationInfo>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        /// <summary>
        /// DS cac vien,trung tam
        /// </summary>
        /// <returns></returns>
        public static async Task<List<OrganizationInfo>> GetListInstitues()
        {
            List<OrganizationInfo> list = new List<OrganizationInfo>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/organize/getinstitues";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<OrganizationInfo>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        public static async Task<List<TaiKhoan>> GetListTaiKhoan(int department)
        {
            List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + $"/api_v2/staffuser/getusers?departmentid={department}";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            listTaiKhoan = JsonConvert.DeserializeObject<List<TaiKhoan>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return listTaiKhoan;
        }

        public static async Task<List<GiangVienAPI>> GetListGiangVien(int departmentid = 0)
        {
            List<GiangVienAPI> list = new List<GiangVienAPI>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/staff/getstave?departmentid=" + departmentid;
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<GiangVienAPI>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        public static async Task<List<GiangVienAPI>> GetListGiangVien(int departmentid = 0, string keyword = "", int page = 1, int pagesize = 30, int degreeId = 0)
        {
            List<GiangVienAPI> list = new List<GiangVienAPI>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + $"/api_v2/staff/getstave?departmentid={departmentid}&keyword={keyword}&page={page}&pagesize={pagesize}&degreeId={degreeId}";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<GiangVienAPI>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }

        public static async Task<GiangVienDetailAPI> GetThongTinGiangVien(int id)
        {
            GiangVienDetailAPI list = new GiangVienDetailAPI();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/staff/getstaff?id=" + id;
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<GiangVienDetailAPI>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }


        public static async Task<List<ChucDanhAPI>> GetListChucDanh()
        {
            List<ChucDanhAPI> list = new List<ChucDanhAPI>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/staff/getTitles";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<ChucDanhAPI>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        public static async Task<List<ChucVuAPI>> GetListChucVu()
        {
            List<ChucVuAPI> list = new List<ChucVuAPI>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/staff/getPositions";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<ChucVuAPI>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        //API: LẤY DANH SÁCH NĂM HỌC	
        public static async Task<List<Years>> GetYears()
        {
            List<Years> list = new List<Years>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL + "/api_v2/school/getYears";

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<Years>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }

        //API: LẤY DANH SÁCH HỆ ĐÀO TẠO
        public static async Task<List<HeDaoTao>> GetGrades()
        {
            List<HeDaoTao> list = new List<HeDaoTao>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL + "/api_v2/school/getGrades";

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<HeDaoTao>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        //API: LẤY DANH SÁCH HỌC KỲ
        public static async Task<List<HocKy>> GetSemester(int yearId)
        {
            List<HocKy> list = new List<HocKy>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL + $"/api_v2/school/getSemester?yearId={yearId}";

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<HocKy>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        //API: LẤY DANH SÁCH DANH MỤC QUY ĐỔI GIẢNG DẠY		
        public static async Task<List<DanhMucQuyDoiGiangDay>> GetCategories()
        {
            List<DanhMucQuyDoiGiangDay> list = new List<DanhMucQuyDoiGiangDay>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL + "/api_v2/teaching/getCategories";

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<DanhMucQuyDoiGiangDay>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        //API: LẤY DANH SÁCH QUY ĐỔI GIẢNG DẠY
        public static async Task<List<QuyDoiGiangDay>> GetConvertions()
        {
            List<QuyDoiGiangDay> list = new List<QuyDoiGiangDay>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL + "/api_v2/teaching/getConversions";

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<QuyDoiGiangDay>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }
        public static async Task<List<HocHamHocViAPI>> GetListHocHamHocVi()
        {
            List<HocHamHocViAPI> list = new List<HocHamHocViAPI>();
            using (var client = new HttpClient())
            {
                try
                {
                    string url = string.Empty;
                    url = BASE_URL_API + "/api_v2/staff/getDegrees";
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    HttpResponseMessage request = await client.GetAsync(url);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            list = JsonConvert.DeserializeObject<List<HocHamHocViAPI>>(response.data.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return list;
        }


        //public static async Task<TaiKhoan> Login(string userName, string passWord)
        public static async Task<string> Login()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpContent c = new StringContent(string.Empty, Encoding.UTF8);
                    //specify to use TLS 1.2 as default connection
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

                    //Client thực hiện đăng nhập trên hệ thống SSO HNUE-ID
                    var request1 = await client.PostAsync($"{ApiBase}/OAuth/Login?returnUrl={home}", c);
                    if (request1.IsSuccessStatusCode)
                    {
                        //Redirect về địa chỉ nhận kết quả kèm accessToken nếu người dùng đăng nhập thành công
                        //Mã truy cập xác thực phía SERVER
                        //var request2 = await client.PostAsync($"{ApiBase}/OAuth/Login?returnUrl={home}&username={userName}&password={passWord}", c);
                        //if (request2.IsSuccessStatusCode)
                        //{
                        //    var json = await request2.Content.ReadAsStringAsync();
                        //    var response = JsonConvert.DeserializeObject<clsRespon>(json);
                        //    if (response.success == "true")
                        //    {
                        //        var respon = JsonConvert.DeserializeObject<AccountInfo>(response.data.ToString());

                        //        string accessToken = "";
                        //        var request3 = await client.PostAsync($"{ApiBase}/OAuth/getUser?accessToken={accessToken}&appToken={accessToken}", c);
                        //        if (request3.IsSuccessStatusCode)
                        //        {
                        //            json = await request3.Content.ReadAsStringAsync();
                        //            response = JsonConvert.DeserializeObject<clsRespon>(json);
                        //            if (response.success == "true")
                        //            {
                        //                var taikhoan = JsonConvert.DeserializeObject<AccountInfo>(response.data.ToString());
                        //                return taikhoan;
                        //            }
                        //        }
                        //    }
                        //}
                        var json = request1.RequestMessage.RequestUri.ToString();
                        return json;

                    }

                    //return new TaiKhoan() { Username = userName };
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            #region save

            //using (var client = new HttpClient())
            //{
            //    try
            //    {
            //        HttpContent c = new StringContent(string.Empty, Encoding.UTF8);

            //        var request = await client.PostAsync($"{BASE_URL_API}/api_v2/staffuser/validate?username={userName}&password={passWord}", c);

            //        if (request.IsSuccessStatusCode)
            //        {
            //            var json = await request.Content.ReadAsStringAsync();
            //            clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
            //            if (response.success == "true")
            //            {
            //                var taikhoan = JsonConvert.DeserializeObject<TaiKhoan>(response.data.ToString());
            //                return taikhoan;
            //            }
            //        }
            //        return new TaiKhoan() { Username = userName };
            //    }
            //    catch (Exception ex)
            //    {
            //        return null;
            //    }
            //}

            #endregion
        }
        public static async Task<TaiKhoan> ValidateLogin(string accessToken)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpContent c = new StringContent(string.Empty, Encoding.UTF8);

                    client.DefaultRequestHeaders.Add("Token", appToken);
                    var request3 = await client.PostAsync($"{ApiBase}/OAuth/getUser?accessToken={accessToken}", c);
                    if (request3.IsSuccessStatusCode)
                    {
                        var json = await request3.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            var taikhoan = JsonConvert.DeserializeObject<TaiKhoan>(response.data.ToString());
                            return taikhoan;
                        }
                    }
                    return new TaiKhoan() { Username = "" };
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }
        public static async Task<bool> ResetPassword(string userId, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpContent c = new StringContent(string.Empty, Encoding.UTF8);
                    client.DefaultRequestHeaders.Add("token", token_auth);

                    var request = await client.PostAsync($"{BASE_URL_API}/api_v2/staffuser/changepassword?userId={userId}&newpassword={password}", c);
                    if (request.IsSuccessStatusCode)
                    {
                        var json = await request.Content.ReadAsStringAsync();
                        clsRespon response = JsonConvert.DeserializeObject<clsRespon>(json);
                        if (response.success == "true")
                        {
                            var result = JsonConvert.DeserializeObject<bool>(response.data.ToString());
                            return result;
                        }
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}