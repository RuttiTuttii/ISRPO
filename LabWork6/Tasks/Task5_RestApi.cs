using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LabWork6.Tasks;

public static class Task5_RestApi
{
    public static void Run(string[] args)
    {
        Console.WriteLine("=== Задание 5.5: REST API (ASP.NET Core) ===");
        Console.WriteLine("Запуск локального HTTP сервера на http://localhost:5000 ...");

        var builder = WebApplication.CreateBuilder(args);
        // принудительно слушаем HTTP на localhost без HTTPS
        builder.WebHost.UseUrls("http://localhost:5000");

        var app = builder.Build();

        // тестовая база пользователей
        var mockUsers = new Dictionary<int, object>
        {
            [1] = new { id = 1, name = "Иван Иванов", role = "Студент" },
            [2] = new { id = 2, name = "Егор Смирнов", role = "Разработчик" },
            [3] = new { id = 3, name = "Алексей Петров", role = "Преподаватель" }
        };

        // middleware глобального логирования исключений (п. 5.5.2)
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "[REST API Ошибка] {Message}", ex.Message);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Internal Server Error",
                    message = ex.Message,
                    statusCode = 500
                });
            }
        });

        // главная страница со списком доступных ссылок
        app.MapGet("/", () => Results.Content(
            "<h2>ЛР №6 — Задание 5.5: REST API</h2>" +
            "<ul>" +
            "<li><a href='/user?id=1'>/user?id=1</a> — 200 OK</li>" +
            "<li><a href='/user?id=999'>/user?id=999</a> — 404 Not Found</li>" +
            "<li><a href='/user?id=abc'>/user?id=abc</a> — 400 Bad Request JSON</li>" +
            "<li><a href='/user/crash'>/user/crash</a> — 500 Server Error</li>" +
            "</ul>", "text/html; charset=utf-8"));

        // эндпоинт GET /user с валидацией id (п. 5.5.1)
        app.MapGet("/user", (HttpContext context) =>
        {
            string? rawId = context.Request.Query["id"];

            // если id не передан или не является числом — возвращаем 400 JSON
            if (string.IsNullOrWhiteSpace(rawId) || !int.TryParse(rawId, out int userId))
            {
                return Results.Json(new
                {
                    error = "Invalid ID format",
                    statusCode = 400
                }, statusCode: 400);
            }

            // если пользователя с таким id нет — возвращаем 404
            if (!mockUsers.TryGetValue(userId, out var user))
            {
                return Results.Json(new
                {
                    error = "User not found",
                    statusCode = 404
                }, statusCode: 404);
            }

            // пользователь найден — 200 OK
            return Results.Ok(user);
        });

        // эндпоинт для проверки перехвата сбоя (500)
        app.MapGet("/user/crash", () =>
        {
            throw new InvalidOperationException("Имитация сбоя для проверки middleware");
        });

        Console.WriteLine("Сервер запущен. Ссылки для проверки:");
        Console.WriteLine("  -> http://localhost:5000/user?id=1");
        Console.WriteLine("  -> http://localhost:5000/user?id=abc");
        Console.WriteLine("  -> http://localhost:5000/user?id=999");
        Console.WriteLine("\nДля остановки сервера нажмите Ctrl+C в консоли.");

        app.Run();
    }
}
