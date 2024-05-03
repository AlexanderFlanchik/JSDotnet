namespace JSDotnet.DTOs
{
    public class RenderComponentResult
    {
        public bool IsSuccess => Exception is null;
        
        /// <summary>
        /// HTML content or JSON payload
        /// </summary>
        public string? Content { get; set; }
        
        /// <summary>
        /// If is not null, represents the URL for redirect.
        /// </summary>
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// For rendering, specifies the content type
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Exception which may be thrown during rendering
        /// </summary>
        public Exception? Exception { get; set; }
    }
}