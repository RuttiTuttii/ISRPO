namespace LabWork6.Tasks;

public static class Task5_RestApi
{
    public static void Run(string[] args)
    {
        Console.WriteLine("--- Задание 5.5: REST API Сервис (ASP.NET Core) ---\n");
        Console.WriteLine("запускаем веб-сервер Kestrel на http://localhost:5000 ...");

        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://localhost:5000");

        var app = builder.Build();

        // тестовые пользователи сервиса
        var mockUsers = new Dictionary<int, object>
        {
            [1] = new { id = 1, name = "Иван Иванов", role = "Студент", group = "ИСПП-31" },
            [2] = new { id = 2, name = "Егор Смирнов", role = "Разработчик", group = "ИСПП-31" },
            [3] = new { id = 3, name = "Алексей Петров", role = "Преподаватель", department = "ИСТ" }
        };

        // middleware глобального перехвата и логирования исключений (п. 5.5.2)
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "[REST API] Необработанная ошибка на {Path}", context.Request.Path);
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

        // главная страница с подсказками
        app.MapGet("/", () => Results.Content(
            "<h2>ЛР №6 — Задание 5.5: REST API Обработка исключений</h2>" +
            "<p>Ссылки для проверки работы сервиса:</p>" +
            "<ul>" +
            "<li><a href='/user?id=1' target='_blank'>/user?id=1</a> — Успешный запрос (200 OK)</li>" +
            "<li><a href='/user?id=999' target='_blank'>/user?id=999</a> — Пользователь не найден (404 Not Found)</li>" +
            "<li><a href='/user?id=abc' target='_blank'>/user?id=abc</a> — Неверный формат ID (400 Bad Request JSON)</li>" +
            "<li><a href='/user/crash' target='_blank'>/user/crash</a> — Тест сбоя сервера (500 Internal Error)</li>" +
            "</ul>", "text/html; charset=utf-8"));

        // эндпоинт GET /user с валидацией параметра id (п. 5.5.1)
        app.MapGet("/user", (HttpContext context) =>
        {
            string? rawId = context.Request.Query["id"];
            app.Logger.LogInformation("GET /user id='{RawId}'", rawId);

            // если id не число или пустой — возвращаем HTTP 400 со схемой JSON
            if (string.IsNullOrWhiteSpace(rawId) || !int.TryParse(rawId, out int userId))
            {
                return Results.Json(new
                {
                    error = "Invalid ID format",
                    statusCode = 400
                }, statusCode: 400);
            }

            // если юзер не найден — возвращаем HTTP 404
            if (!mockUsers.TryGetValue(userId, out var user))
            {
                return Results.Json(new
                {
                    error = "User not found",
                    statusCode = 404
                }, statusCode: 404);
            }

            // возвращаем найденного юзера со статусом 200 OK
            return Results.Ok(user);
        });

        // ручка для проверки сбоя 500
        app.MapGet("/user/crash", () =>
        {
            throw new InvalidOperationException("Имитация критического сбоя внутри REST API");
        });

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("сервер успешно запущен на http://localhost:5000");
        Console.WriteLine("для остановки сервера нажмите Ctrl+C в консоли.\n");
        Console.ResetColor();

        app.Run();
    }
}
