using BootstrapBlazor.Components;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SX.TSSR.Application.Features.ClosingRateStatistics.Queries.GetClosureRateOfRegionalIssues;
using SX.TSSR.Application.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SX.TSSR.Web.Pages.ClosingRateStatistics.ClosureRateOfRegionalIssues
{
    public sealed partial class ClosureRateOfRegionalIssues
    {
        /// <summary>
        /// 区域问题关闭
        /// </summary>
        private IEnumerable<ClosureRateOfRegionalIssuesDetailVm> vmDetail = null;   //问题明细
        private IEnumerable<ClosureRateOfRegionalIssuesDetailVm> vmSubTotal = null;     //问题小计
        private IEnumerable<ClosureRateOfRegionalIssuesDetailVm> vmTotal = null;        //问题汇总

        /// <summary>
        /// 是否加载成功
        /// </summary>
        private bool loadFailed = false;

        /// <summary>
        /// 查询条件
        /// </summary>
        private ClosureRateOfRegionalIssuesQueryModel QueryModel { get; set; } = new ClosureRateOfRegionalIssuesQueryModel()
        {
            BeginDate = DateTime.Now,
            EndDate = DateTime.Now,
            Trade = "1"
        };

        private static readonly ConcurrentDictionary<Type, Func<IEnumerable<ClosureRateOfRegionalIssuesDetailVm>, string, SortOrder, IEnumerable<ClosureRateOfRegionalIssuesDetailVm>>> SortLambdaCache = new ConcurrentDictionary<Type, Func<IEnumerable<ClosureRateOfRegionalIssuesDetailVm>, string, SortOrder, IEnumerable<ClosureRateOfRegionalIssuesDetailVm>>>();

        private Task<QueryData<ClosureRateOfRegionalIssuesDetailVm>> OnQueryAsync(QueryPageOptions options) => BindItemQueryAsync(vmDetail, options);

        private Task<QueryData<ClosureRateOfRegionalIssuesDetailVm>> BindItemQueryAsync(IEnumerable<ClosureRateOfRegionalIssuesDetailVm> items, QueryPageOptions options)
        {
            // 过滤
            var isFiltered = false;
            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<ClosureRateOfRegionalIssuesDetailVm>());

                // 通知内部已经过滤数据了
                isFiltered = true;
            }

            // 排序
            var isSorted = false;
            if (!string.IsNullOrEmpty(options.SortName))
            {
                // 外部未进行排序，内部自动进行排序处理
                var invoker = SortLambdaCache.GetOrAdd(typeof(ClosureRateOfRegionalIssuesDetailVm), key => items.GetSortLambda().Compile());
                items = invoker(items, options.SortName, options.SortOrder);

                // 通知内部已经过滤数据了
                isSorted = true;
            }

            // 设置记录总数
            var total = items.Count();

            return Task.FromResult(new QueryData<ClosureRateOfRegionalIssuesDetailVm>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = isSorted,
                IsFiltered = isFiltered,
                //IsSearch = false
            });

        }

        protected override async Task OnInitializedAsync()
        {
            await QueryAsync();
            await base.OnInitializedAsync();
        }      

        private async Task QueryAsync()
        {
            try
            {
                loadFailed = false;
                vmDetail = null;
                vmSubTotal = null;

                var result = await _mediator.Send(new GetClosureRateOfRegionalIssuesDetailQuery() { QueryModel = QueryModel });

                vmDetail = result;

                vmSubTotal = result.GroupBy(t => new { t.Trade, t.DealWither })
                                .Select(g => new ClosureRateOfRegionalIssuesDetailVm
                {
                    Trade = g.Key.Trade,
                    DealWither = g.Key.DealWither,
                    Provice = "",
                    RegionQuestionNum = g.Sum(x => x.RegionQuestionNum),
                    NoMoidficationQuestionNum = g.Sum(x => x.NoMoidficationQuestionNum),
                    HandledQuestionNum = g.Sum(x => x.HandledQuestionNum),
                    AssistHandledQuestionNum = g.Sum(x => x.AssistHandledQuestionNum),
                    ClosedQuestionNum = g.Sum(x => x.ClosedQuestionNum),
                    RegionClosedQuestionRate = ((double)g.Sum(x => x.ClosedQuestionNum) * 100/ 
                                            (double)(g.Sum(x => x.RegionQuestionNum) == 0 ? 1 : (double)g.Sum(x => x.RegionQuestionNum))).ToString("#0.00") + "%",
                    HandledQuestionRate = ((double)g.Sum(x => x.HandledQuestionNum) * 100 / 
                                            (double)(g.Sum(x => x.RegionQuestionNum) == 0 ? 1 : (double)g.Sum(x => x.RegionQuestionNum))).ToString("#0.00") + "%"
                });

                vmTotal = result.GroupBy(t => t.Trade)
                            .Select(g => new ClosureRateOfRegionalIssuesDetailVm
                            {
                                Trade = g.Key,
                                DealWither = "",
                                Provice = "",
                                RegionQuestionNum = g.Sum(x => x.RegionQuestionNum),
                                NoMoidficationQuestionNum = g.Sum(x => x.NoMoidficationQuestionNum),
                                HandledQuestionNum = g.Sum(x => x.HandledQuestionNum),
                                AssistHandledQuestionNum = g.Sum(x => x.AssistHandledQuestionNum),
                                ClosedQuestionNum = g.Sum(x => x.ClosedQuestionNum),
                                RegionClosedQuestionRate = ((double)g.Sum(x => x.ClosedQuestionNum) * 100 /
                                            (double)(g.Sum(x => x.RegionQuestionNum) == 0 ? 1 : (double)g.Sum(x => x.RegionQuestionNum))).ToString("#0.00") + "%",
                                HandledQuestionRate = ((double)g.Sum(x => x.HandledQuestionNum) * 100 /
                                            (double)(g.Sum(x => x.RegionQuestionNum) == 0 ? 1 : (double)g.Sum(x => x.RegionQuestionNum))).ToString("#0.00") + "%"
                            });
            }
            catch (Exception ex)
            {
                loadFailed = true;
                System.Console.WriteLine(ex);
            }
        }

    }
}
