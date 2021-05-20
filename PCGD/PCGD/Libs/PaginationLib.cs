using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCGD.Libs
{
    public class PaginationLib
    {
        private static int limit;
        static PaginationLib()
        {
            limit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationShowPage"]);
        }
        private static string Url(string text = "", int page = 1)
        {
            return (text == "" ? "?page=" + page : "?text=" + text + "&page=" + page);
        }
        public static string Get(string text = "", int total = 0, int page = 1)
        {
            string content = "";
            total = (int)Math.Ceiling((decimal)(total / Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PaginationLimit"])));
            if (page > 1)
                content += "<li class=\"page-item\"><a class=\"page-link\" href=\"" + Url(text, 1) + "\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>";
            for (int i = (page - limit > 0 ? page - limit : 1); i <= (page + limit < total ? page + limit : total); i++)
                content += (page == i ? "<li class=\"page-item active\" aria-current=\"page\"><a class=\"page-link\" href=\"javascript:void(0)\">" + i + "</a></li>" : "<li class=\"page-item\"><a class=\"page-link\" href=\"" + Url(text, i) + "\">" + i + "</a></li>");
            if (page < total)
                content += "<li class=\"page-item\"><a class=\"page-link\" href=\"" + Url(text, total) + "\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span></a></li>";
            return "<nav class=\"mt-3\" aria-label=\"Page navigation example\"><ul class=\"pagination mb-0 justify-content-center\">" + content + "</ul></nav>";
        }
    }
}