using MediatR;

namespace Configuration.Application.Queries.ListNotification
{
    public class ListNotification : IRequest<string>
    {
        public bool isEnglish { get; set; }
    }
}
