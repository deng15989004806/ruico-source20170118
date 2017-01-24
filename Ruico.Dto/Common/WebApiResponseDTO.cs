namespace Ruico.Dto.Common
{
    public class WebApiResponseDTO<T>
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}