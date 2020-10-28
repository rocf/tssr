using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SX.TSSR.Application.Models
{
    public class ClosureRateOfRegionalIssuesQueryModel
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required(ErrorMessage = "开始日期不可以为空")]
        [DisplayName("开始日期")]
        [DataType(DataType.Date)]
        [EditorOrder(1)]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required(ErrorMessage = "结束日期不可以为空")]
        [DisplayName("结束日期")]
        [DataType(DataType.Date)]
        [EditorOrder(2)]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        [Required(ErrorMessage = "行业")]
        [DisplayName("行业")]
        [EditorOrder(3)]
        public string Trade { get; set; }
    }
}
