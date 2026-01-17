using GNA.API.Requests;

namespace GNA.API.Interfaces
{
    public interface IMcpService
    {
        string HikayePromptOlustur(StoryRequest request);
    }
}
