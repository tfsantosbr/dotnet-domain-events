namespace Shope.Application.Base.Reports;

public interface IReportService
{
    Task RegisterReportAsync(string reportName, string reportContent);
}
