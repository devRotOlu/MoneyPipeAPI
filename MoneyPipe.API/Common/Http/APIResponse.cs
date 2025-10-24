﻿namespace MoneyPipe.API.Common.Http
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Ok(T? data, string? message = null)
            => new() { Success = true, Message = message, Data = data };

        public static ApiResponse<T> Fail(string message, object? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }
}
