using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddCheck<RequestTimeHealthCheck>("RequestTimeCheck");  // ��������� ����������������� � RequestTimeCheck

builder.Services.AddHttpClient();   // ���������� HttpClient
builder.WebHost.UseUrls("https://[::]:44444");  // ������������ ������� �� ������ https://localhost:44444

var app = builder.Build();
app.MapHealthChecks("/health");

app.MapGet("/", async (HttpClient httpClient) =>
{
    // ���������� ������ � ������� ������� � ���������� ��� �����
    var response = await httpClient.GetAsync("https://localhost:33333/data");
    return await response.Content.ReadAsStringAsync();
});

app.Run();

public class RequestTimeHealthCheck : IHealthCheck
{
    int degraded_level = 2000;  // ������� ������ ������
    int unhealthy_level = 5000; // ��������� �������
    HttpClient httpClient;
    public RequestTimeHealthCheck(HttpClient client) => httpClient = client;
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // �������� ����� �������
        Stopwatch sw = Stopwatch.StartNew();
        await httpClient.GetAsync("https://localhost:33333/data");
        sw.Stop();
        var responseTime = sw.ElapsedMilliseconds;
        // � ����������� �� ������� ������� ���������� ������������ ���������
        if (responseTime < degraded_level)
        {
            return HealthCheckResult.Healthy("������� ������������� ������");
        }
        else if (responseTime < unhealthy_level)
        {
            return HealthCheckResult.Degraded("�������� �������� ������ �������");
        }
        else
        {
            return HealthCheckResult.Unhealthy("������� � ��������� ���������. ��������� �� ����������.");
        }
    }
}