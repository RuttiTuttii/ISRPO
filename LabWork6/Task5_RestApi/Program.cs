var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// словарь с тестовой базой пользователей для демонстрации ответов сервиса
var mockUsers = new Dictionary<int, object>
{
    [1] = new { id = 1, name = "Иван Иванов", role = "Студент", group = "ИСПП-31" },
    [2] = new { id = 2, name = "Егор Смирнов", role = "Разработчик", group = "ИСПП-31" },
    [3] = new { id = 3, name = "Алексей Петров", role = "Преподаватель", department = "ИСТ" }
};

// пункт 5.5.2: глобальный middleware для логирования всех непредвиденных исключений
app.Use(async (context, next) =>
{
    try
    {
        // передаем запрос дальше по конвейеру обработки
        await next();
    }
    catch (Exception ex)
    {
        // логируем возникшее в сервисе исключение в консоль сервера
        app.Logger.LogError(ex, "[REST API Ошибка] Необработанное исключение при обработке запроса {Path}", context.Request.Path);

        // возвращаем клиенту стандартизированный ответ 500 Internal Server Error в формате JSON
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

// главная страница с подсказками по проверке эндпоинтов
app.MapGet("/", () => Results.Content(
    "<h2>ЛР №6 — Задание 5.5: REST API Обработка исключений</h2>" +
    "<p>Проверьте следующие запросы:</p>" +
    "<ul>" +
    "<li><a href='/user?id=1' target='_blank'>/user?id=1</a> — Успешный запрос (200 OK)</li>" +
    "<li><a href='/user?id=999' target='_blank'>/user?id=999</a> — Пользователь не найден (404 Not Found)</li>" +
    "<li><a href='/user?id=abc' target='_blank'>/user?id=abc</a> — Неверный формат ID (400 Bad Request JSON)</li>" +
    "<li><a href='/user/crash' target='_blank'>/user/crash</a> — Тест сбоя сервера (500 Internal Error)</li>" +
    "</ul>", "text/html; charset=utf-8"));

// пункт 5.5.1: эндпоинт GET /user с валидацией параметра id и кодами 400/404
app.MapGet("/user", (HttpContext context) =>
{
    // извлекаем query-параметр id из строки запроса
    string? rawId = context.Request.Query["id"];

    // логируем входящий запрос для трассировки
    app.Logger.LogInformation("Получен GET-запрос /user с параметром id='{RawId}'", rawId);

    // если параметр id отсутствует или его невозможно преобразовать в целое число — отдаем 400 JSON
    if (string.IsNullOrWhiteSpace(rawId) || !int.TryParse(rawId, out int userId))
    {
        app.Logger.LogWarning("Некорректный формат id: '{RawId}' -> отдаем HTTP 400 JSON", rawId);
        return Results.Json(new
        {
            error = "Invalid ID format",
            statusCode = 400
        }, statusCode: 400);
    }

    // ищем пользователя в нашей локальной базе
    if (!mockUsers.TryGetValue(userId, out var user))
    {
        app.Logger.LogWarning("Пользователь с id={UserId} не найден -> отдаем HTTP 404", userId);
        return Results.Json(new
        {
            error = "User not found",
            statusCode = 404
        }, statusCode: 404);
    }

    // если все проверки пройдены — возвращаем данные пользователя со статусом 200 OK
    return Results.Ok(user);
});

// вспомогательный эндпоинт для проверки глобального перехвата ошибок middleware (п. 5.5.2)
app.MapGet("/user/crash", () =>
{
    // бросаем искусственное исключение для проверки глобального перехвата
    throw new InvalidOperationException("Имитация критического сбоя внутри REST API сервиса");
});

app.Run();
