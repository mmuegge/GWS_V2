using Microsoft.AspNetCore.Components;

namespace GWS_Statistics.Components.Services
{
    public class ToastOption
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
