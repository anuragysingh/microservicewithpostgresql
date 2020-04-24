namespace Customer.API.Middlewares
{
    using Newtonsoft.Json;

    public class APIErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
