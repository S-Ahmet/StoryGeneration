using System.Threading.Tasks;
using GNA.API.Requests;
using GNA.API.Responses;

namespace GNA.API.Interfaces
{
    public interface IStoryService
    {
        Task<StoryResponse> HikayeUret(StoryRequest request);
    }
}
