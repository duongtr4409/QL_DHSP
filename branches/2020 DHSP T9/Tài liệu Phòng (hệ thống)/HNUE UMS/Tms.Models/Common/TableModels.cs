using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;

namespace Ums.Models.Common
{
    [ModelBinder(typeof(DtModelBinder))]
    public class TableModel
    {
        public int Draw { get; set; }
        public int Pagesize { get; set; }
        public int Start { get; set; }
        public string Order { get; set; }
    }

    public class DtModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            var orderindex = request.QueryString["order[0][column]"].ToInt();
            var r = new TableModel
            {
                Order = request.QueryString[$"columns[{orderindex}][data]"] + " " + request.QueryString["order[0][dir]"],
                Pagesize = request.QueryString["length"].ToInt(),
                Start = request.QueryString["start"].ToInt(),
                Draw = request.QueryString["draw"].ToInt()
            };
            return r;
        }
    }

    public static class Extensions
    {
        public static TableResult<T> ToTableResult<T>(this IQueryable<T> source, int count, int draw)
        {
            return new TableResult<T>
            {
                draw = draw,
                data = source.ToList(),
                recordsFiltered = count,
                recordsTotal = count
            };
        }
        public static TableResult<T> ToTableResult<T>(this List<T> source, int count, int draw)
        {
            return new TableResult<T>
            {
                draw = draw,
                data = source.ToList(),
                recordsFiltered = count,
                recordsTotal = count
            };
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TableResult<T>
    {
        public int draw { get; set; }
        public IEnumerable<T> data { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }
}