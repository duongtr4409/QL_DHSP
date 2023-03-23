using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.Core.Domain.Connect;
using Ums.Models.Common;
using Ums.Models.Connect;
using Ums.Services.Connect;
using Ums.Services.Users;

namespace Ums.Website.Controllers
{
    public class ConnectController : BaseController
    {
        private readonly INoticeService _noticeService;
        private readonly IFeedbackService _feedbackService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        public ConnectController(INoticeService noticeService, IFeedbackService feedbackService, IMessageService messageService, IUserService userService)
        {
            _noticeService = noticeService;
            _feedbackService = feedbackService;
            _messageService = messageService;
            _userService = userService;
        }

        public ActionResult Message()
        {
            return View();
        }

        public JsonResult GetUsers()
        {
            var _ids = _messageService.Gets().Where(i => i.FromId == WorkContext.UserInfo.Id || i.ToId == WorkContext.UserInfo.Id).SelectMany(i => new[] { new SortId { Id = i.FromId, Date = i.Created }, new SortId { Id = i.ToId, Date = i.Created } }).ToList();
            var ids = _ids.OrderByDescending(i => i.Date).Select(i => i.Id).ToList();
            var users = _userService.Gets().Where(i => i.Id != WorkContext.UserInfo.Id && ids.Contains(i.Id))
                .Select(i => new { i.Id, i.Name, i.UserType, i.Email, i.Avatar }).ToList()
                .OrderBy(i => ids.IndexOf(i.Id)).ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMessages(int userId, int lastId)
        {
            var messages = _messageService.Gets(groupIds: new[] { userId, WorkContext.UserInfo.Id }).Where(i => i.Id > lastId).OrderBy(i => i.Created).ToList()
                .Select(i => new
                {
                    i.FromId,
                    i.From.Name,
                    i.Content,
                    Date = i.Created.ToAppDateTime(),
                    i.Id,
                    i.From.Avatar
                });
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchUsers(string keyword)
        {
            //var users = _userService.Gets().Where(i => ( i.Name.Contains(keyword) || i.Email == keyword ) && i.Id != WorkContext.UserInfo.Id).Select(i => new { i.Id, i.Email, i.Name, i.UserType, i.Avatar }).ToList();
            keyword = SearchUtils.RemoveSign4VietnameseString(keyword.Trim().ToLower());
            var users = _userService.Gets().Where(i => i.Id != WorkContext.UserInfo.Id).Select(i => new { i.Id, i.Email, i.Name, i.UserType, i.Avatar }).ToList().Where(i =>
                SearchUtils.searchVietNameseString(i.Name, keyword) || SearchUtils.searchVietNameseString(i.Email, keyword)
            );
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public void SendMessage(string message, int toId)
        {
            _messageService.Insert(new Message
            {
                Content = message,
                FromId = WorkContext.UserInfo.Id,
                ToId = toId
            });
        }

        public ActionResult Notice()
        {
            return View();
        }

        public object GetNotice(TableModel model)
        {
            var lst = _noticeService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Department = i.Department.Name,
                    Staff = i.Staff.Name,
                    i.Public
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult NoticeView(int id = 0)
        {
            var m = _noticeService.Get(id);
            return View(m);
        }
    }

    public class SortId
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class SearchUtils
    {
        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }


        public static bool searchVietNameseString(string data, string key)      // tham số key đã lowerCase và loại bỏ ký tự tiếng việt
        {
            try
            {
                if (data == null && key == null) return true;
                else if (data == null) return false;
                else
                {
                    return RemoveSign4VietnameseString(data.ToLower()).Contains(key);
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

    }
}