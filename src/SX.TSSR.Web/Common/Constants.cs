using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SX.TSSR.Web.Common
{
    public class Constants
    {

        public static IEnumerable<SelectedItem> TradeSelectedItems = new SelectedItem[]
        {
            new SelectedItem("-1", "全部"),
            new SelectedItem("1", "商超"),
            new SelectedItem("2", "餐饮"),
            new SelectedItem("3", "专卖"),
            new SelectedItem("6", "eShop"),
            new SelectedItem("8", "商锐"),
            new SelectedItem("C", "生鲜便利"),
            new SelectedItem("I", "星食客"),
            new SelectedItem("L", "美食家云"),
            new SelectedItem("G", "O2O"),
            new SelectedItem("M", "移动及支付")
        };


    }
}
