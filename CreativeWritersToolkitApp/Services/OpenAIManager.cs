using CreativeWritersToolkitApp.Models;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeWritersToolkitApp.Services
{
    public class OpenAIManager
    {
        private readonly Prompt prompt;

        public OpenAIManager(Prompt prompt)
        {
            this.prompt = prompt;
        }

        private async Task<CompletionCreateResponse> BuildPrompt(OpenAIService sdk)
        {
            var fullPrompt = string.Empty;
            if (prompt.PEX != null)
            {
                fullPrompt = prompt.PEX + prompt.PromptText;
            }
            else fullPrompt = prompt.PromptText;

            var request = new CompletionCreateRequest();
            request.Prompt = fullPrompt;
            request.Temperature = prompt.Temperature;
            request.TopP = prompt.Top_P;
            request.FrequencyPenalty = prompt.Frequency_Penalty;
            request.PresencePenalty = prompt.Presence_Penalty;
            request.BestOf = 1;
            request.MaxTokens = prompt.Max_Tokens;

            if (request.Stop != null)
                request.Stop = prompt.Stop[0];

            var result = await sdk.Completions.Create(request, OpenAI.GPT3.ObjectModels.Models.Model.TextDavinciV2);
            return result;
                
        }

        public async Task<string> Run(ApiFile api)
        {
            var sdk = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = api.API,
            });

            var request = new CompletionCreateRequest();
            request.Prompt = prompt.PromptText;
            request.Temperature = prompt.Temperature;
            request.TopP = prompt.Top_P;
            request.FrequencyPenalty = prompt.Frequency_Penalty;
            request.PresencePenalty = prompt.Presence_Penalty;
            request.BestOf = 1;
            request.MaxTokens = prompt.Max_Tokens;
            if (request.Stop != null)
                request.Stop = prompt.Stop[0];


            var result = await sdk.Completions.Create(request, OpenAI.GPT3.ObjectModels.Models.Model.TextDavinciV2);

            if (result.Successful)
                return result.Choices.FirstOrDefault().Text;
            else
            {

                return result.Error.ToString();
            }
        }
    }
}
