namespace Client.Scripts.Core.AI
{
    internal struct GenerativeModel
    {
        public string EndpointFormat { get; }
        public string ModelName { get; }
        public string ApiKey { get; }

        private GenerativeModel(string endpointFormat = null, string modelName = null, string apiKey = null)
        {
            EndpointFormat = endpointFormat;
            ModelName = modelName;
            ApiKey = apiKey;
        }

        //TODO:<Dmitriy.Sukharev> Move to config
        public static GenerativeModel Default => new(
            endpointFormat: "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}",
            modelName: "gemini-1.5-flash-latest",
            apiKey: "AIzaSyCm0tkXkG-rsuENh6HgVdGDeOp8tIZACS4"
        );
    }
}