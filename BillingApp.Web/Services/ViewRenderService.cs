using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

    namespace BillingApp.Web.Services
    {
        public class ViewRenderService
        {
            private readonly ICompositeViewEngine _viewEngine;
            private readonly ITempDataProvider _tempDataProvider;
            private readonly IServiceProvider _serviceProvider;

            public ViewRenderService(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
            {
                _viewEngine = viewEngine;
                _tempDataProvider = tempDataProvider;
                _serviceProvider = serviceProvider;
            }

            public async Task<string> RenderViewToStringAsync(Controller controller, string viewName, object model)
            {
                controller.ViewData.Model = model;

                using (var writer = new StringWriter())
                {
                    var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);

                    if (!viewResult.Success)
                    {
                        throw new InvalidOperationException($"View '{viewName}' not found.");
                    }

                    var viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);
                    return writer.ToString();
                }
            }
        }
    }
