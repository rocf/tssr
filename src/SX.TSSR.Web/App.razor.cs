using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SX.TSSR.Web
{
    public sealed partial class App
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if(firstRender && JSRuntime != null)
            {
                JSRuntime.InvokeVoidAsync("$.loading");
            }
        }
    }
}
