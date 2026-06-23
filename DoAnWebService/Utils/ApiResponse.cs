namespace DoAnWebService.Utlis
{
    public class APIResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
