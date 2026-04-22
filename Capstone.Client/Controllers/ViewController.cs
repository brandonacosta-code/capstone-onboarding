using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Capstone.Client.Controllers
{
    public class ViewController : Controller
    {
        public string Index()
        {
            return RenderViewToString("index", null);
        }

        public string Home()
        {
            return RenderViewToString("home", null);
        }

        public string Grid()
        {
            return RenderViewToString("grid", null);
        }

        public string Product()
        {
            return RenderViewToString("product", null);
        }

		public string Cart()
		{
			return RenderViewToString("cart", null);
		}

		public string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
