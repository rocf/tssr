﻿using Dapper;
using MediatR;
using SX.TSSR.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SX.TSSR.Application.Models;

namespace SX.TSSR.Application.Features.ClosingRateStatistics.Queries.GetClosureRateOfRegionalIssues
{
    public class GetClosureRateOfRegionalIssuesDetailQuery : IRequest<IEnumerable<ClosureRateOfRegionalIssuesDetailVm>>
    {
        public ClosureRateOfRegionalIssuesQueryModel QueryModel { get; set; }
        public class GetClosureRateOfRegionalIssuesDetailQueryHandler : IRequestHandler<GetClosureRateOfRegionalIssuesDetailQuery,
                                                                                IEnumerable<ClosureRateOfRegionalIssuesDetailVm>>
        {

            public GetClosureRateOfRegionalIssuesDetailQueryHandler()
            {

            }

            public async Task<IEnumerable<ClosureRateOfRegionalIssuesDetailVm>> Handle(GetClosureRateOfRegionalIssuesDetailQuery request, CancellationToken cancellationToken)
            {
                using (var conn = new SqlConnection(Constants.SqlServerConnectionString))
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append(@"
WITH t AS (
	SELECT  QADictionary.name AS Trade ,	--行业
			QAUser.name AS DealWither ,		--负责人
            QAQuestion.industry AS Provice ,	--区域
            COUNT(*) AS RegionQuestionNum ,		--区域问题数
            SUM(CASE ISNULL(ModifyCode, 1)
                    WHEN '1' THEN 1
                    ELSE 0
                END) AS NoMoidficationQuestionNum ,		--无修改问题数                 
            SUM(CASE ISNULL(modifycode, 1)
                    WHEN '1'
                    THEN ( CASE handler
                            WHEN dealwither
                            THEN ( CASE ISNULL(ModifyCode, 1)
                                    WHEN '1' THEN 1
                                    ELSE 0
                                    END )
                            ELSE 0
                            END )
                    ELSE 0
                END) AS HandledQuestionNum ,		--负责人处理问题数
                SUM(CASE ISNULL(modifycode, 1)
                    WHEN '1' THEN ( CASE Status
                                    WHEN '4' THEN 1
                                    WHEN '5' THEN 1
                                    ELSE 0
                                    END )
                    ELSE 0
                END) AS ClosedQuestionNum		--问题关闭数
	FROM QAQuestion
    LEFT JOIN QADeptMaintenance ON QAQuestion.version = QADeptMaintenance.version 
	AND QADeptMaintenance.dept IN @Trades
	LEFT JOIN QADictionary ON QADeptMaintenance.dept = QADictionary.no AND QADictionary.type = '1'
	LEFT JOIN QAUser ON QAQuestion.DealWither = QAUser.UserID 
	WHERE QAQuestion.userid NOT LIKE 'v%'	--非VIP大客户代理
	AND addedby NOT LIKE 'siss%'	--非内部提交
	AND QAQuestion.userid NOT LIKE 'siss%'	--非内部测试代理
	AND QAQuestion.userid NOT LIKE '9876'	--非9876代理(历史测试代理?!)
	AND ( Category <> '2' AND Category <> '6'  AND category <> '8')	--非需求类问题
	AND dealwither IS NOT NULL	--负责人不为空

");
                    if (request.QueryModel.Trade != "-1")
                    {
                        sbSql.Append(@"
    AND QADeptMaintenance.dept = @Trade
 ");
                    }

                    sbSql.Append(@"
	AND CONVERT(CHAR(10), QAQuestion.FirstSubmitDate, 121) >=  @BeginDate
    AND CONVERT(CHAR(10), QAQuestion.FirstSubmitDate, 121) <=  @EndDate
	GROUP BY  QAQuestion.industry ,
                    QAUser.name ,
					QADictionary.name
)
SELECT  t.Trade,	--行业
		t.DealWither ,		--负责人
		t.Provice,		--区域
		t.RegionQuestionNum,	--区域问题数
		t.NoMoidficationQuestionNum,	--无修改问题数
		t.HandledQuestionNum,		--负责人处理问题数
        ( t.NoMoidficationQuestionNum - t.HandledQuestionNum ) AS AssistHandledQuestionNum ,		--协助处理问题数
		t.ClosedQuestionNum,		--关闭问题数
        ( CASE WHEN t.NoMoidficationQuestionNum = 0 THEN '0.00%'
               ELSE RTRIM(CONVERT(DECIMAL(18, 2), t.ClosedQuestionNum * 100.0
                          / t.NoMoidficationQuestionNum)) + '%'
          END ) AS RegionClosedQuestionRate ,		--区域问题关闭率
        ( CASE WHEN t.NoMoidficationQuestionNum = 0 THEN '0.00%'
               ELSE RTRIM(CONVERT(DECIMAL(18, 2), t.HandledQuestionNum * 100.0
                          / t.NoMoidficationQuestionNum)) + '%'
          END ) AS HandledQuestionRate			--负责人处理比率
		  FROM t
		  ORDER BY  t.Trade, t.DealWither
");

                    try
                    {
                        var vm = await conn.QueryAsync<ClosureRateOfRegionalIssuesDetailVm>(sbSql.ToString(), new
                        {
                            BeginDate = request.QueryModel.BeginDate,
                            EndDate = request.QueryModel.EndDate,
                            Trade = request.QueryModel.Trade,
                            Trades = Constants.Trades
                        });

                        return vm;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    finally
                    {

                    }
                    return null;
                }
            }
        }
    }
}
