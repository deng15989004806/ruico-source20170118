namespace Ruico.WebHost.Models
{
    public class AjaxResponse
    {
        public AjaxResponse()
        {
        }

        public string RedirectUrl { get; set; }

        public string Message { get; set; }

        public bool ShowMessage { get; set; }

        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public object Data { get; set; }
    }
}