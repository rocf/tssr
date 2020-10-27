using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SX.TSSR.Web.Shared
{
    public sealed partial class NavMenu
    {
        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => CssBuilder.Default("sidebar-content")
            .AddClass("collapse", collapseNavMenu)
            .Build();

        private List<MenuItem> Menus { get; set; } = new List<MenuItem>();

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            InitMenus();
        }

        private Task OnClickMenu(MenuItem item)
        {
            if (!item.Items.Any())
            {
                ToggleNavMenu();
                StateHasChanged();
            }
            return Task.CompletedTask;
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private void InitMenus()
        {
            //首页
            var item = new MenuItem()
            {
                Text = "首页",
                Icon = "fa fa-fw fa-home",
                Url = "/"
            };
            AddHome(item);

            //关闭率统计
            item = new MenuItem()
            {
                Text = "关闭率统计",
                Icon = "fa fa-fw fa-times-circle-o"
            };
            AddClosureRate(item);

        }

        private void AddClosureRate(MenuItem item)
        {
            item.AddItem(new MenuItem()
            {
                Text = "区域问题关闭率统计",
                Url = "ClosureRateOfRegionalIssues"
            });
            AddBadge(item);
        }

        private void AddHome(MenuItem item)
        {
            Menus.Add(item);
        }



        private void AddBadge(MenuItem item, bool append = true, int? count = null)
        {
            item.Component = CreateBadge(count ?? item.Items.Count());
            if (append) Menus.Add(item);
        }

        private DynamicComponent CreateBadge(int count) => DynamicComponent.CreateComponent<Badge>(new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>(nameof(Badge.Color), Color.Info),
            new KeyValuePair<string, object>(nameof(Badge.IsPill), true),
            new KeyValuePair<string, object>(nameof(Badge.ChildContent), new RenderFragment(builder => {
                builder.AddContent(0, count);
            }))
        });
    }
}
