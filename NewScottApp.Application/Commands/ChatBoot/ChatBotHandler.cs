using MediatR;

namespace NewScottApp.Application.Commands.ChatBoot
{
    public class ChatBotHandler : IRequestHandler<ChatBot, string>
    {
        public Task<string> Handle(ChatBot request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Thank You for Using our app \n" +
                "we working hard to complete it \n" +
                "please be patient");
        }
    }
}
