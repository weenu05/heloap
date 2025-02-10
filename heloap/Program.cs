using heloap;

var builder = WebApplication.CreateBuilder();
// ������������� ���� ��� ������������
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
// ��������� ������������ � ������� �������� Logging ���� �� 
// �������� ������� WebApplication
var app = builder.Build();

app.Run(async (context) =>
{
    app.Logger.LogInformation($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
    await context.Response.WriteAsync("Hello World!");
});

app.Run();