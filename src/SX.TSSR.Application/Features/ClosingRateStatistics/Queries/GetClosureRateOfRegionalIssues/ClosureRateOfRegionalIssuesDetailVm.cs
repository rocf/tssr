using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SX.TSSR.Application.Features.ClosingRateStatistics.Queries.GetClosureRateOfRegionalIssues
{
    /// <summary>
    /// 区域问题关闭率ViewModel
    /// </summary>
    public class ClosureRateOfRegionalIssuesDetailVm
    {
        /// <summary>
        /// 行业
        /// </summary>
        [DisplayName("行业")]
        public string Trade { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [DisplayName("负责人")]
        public string DealWither { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [DisplayName("区域")]
        public string Provice { get; set; }

        /// <summary>
        /// 区域问题数
        /// </summary>
        [DisplayName("区域问题数")]
        public int RegionQuestionNum { get; set; }

        /// <summary>
        /// 无修改问题数
        /// </summary>
        [DisplayName("无修改问题数")]
        public int NoMoidficationQuestionNum { get; set; }

        /// <summary>
        /// 负责人处理问题数
        /// </summary>
        [DisplayName("负责人处理问题数")]
        public int HandledQuestionNum { get; set; }

        /// <summary>
        /// 协助处理问题数
        /// </summary>
        [DisplayName("协助处理问题数")]
        public int AssistHandledQuestionNum { get; set; }

        /// <summary>
        /// 区域问题关闭数
        /// </summary>
        [DisplayName("区域问题关闭数")]
        public int ClosedQuestionNum { get; set; }

        /// <summary>
        /// 区域关闭率
        /// </summary>
        [DisplayName("区域关闭率")]
        public double RegionClosedQuestionRate { get; set; }

        /// <summary>
        /// 负责人处理比例
        /// </summary>
        [DisplayName("负责人处理比例")]
        public double HandledQuestionRage { get; set; }

    }
}
