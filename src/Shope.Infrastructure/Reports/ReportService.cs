using Microsoft.Extensions.Logging;
using Shope.Application.Base.Reports;

namespace Shope.Infrastructure.Reports;

public class ReportService(ILogger<ReportService> logger) : IReportService
{
    public async Task RegisterReportAsync(string reportName, string reportContent)
    {
        logger.LogInformation(
            "Registering report '{reportName}' with content '{reportContent}'", 
            reportName, reportContent
            );

        await Task.CompletedTask;
    }
}
