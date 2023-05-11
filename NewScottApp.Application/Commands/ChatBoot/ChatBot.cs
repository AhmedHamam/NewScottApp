using MediatR;

namespace NewScottApp.Application.Commands.ChatBoot
{
    public class ChatBot : IRequest<string>
    {
        public string app { get; set; }
        public int sender { get; set; }
        public int message { get; set; }
        public int group_name { get; set; }
        public int phone { get; set; }
    }
    public class Reply
    {
        public string reply { get; set; }
    }
}
