using Microsoft.AspNetCore.Mvc.Razor;

namespace Give_Aid.Helpers
{
    internal class AdminViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var additionalLocations = new[]
            {
                "/Views/Admin/{1}/{0}.cshtml"
            };

            return additionalLocations.Concat(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // No dynamic values needed for this expander.
        }
    }
}