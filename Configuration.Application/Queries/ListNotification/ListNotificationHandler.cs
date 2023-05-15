using Configuration.Reprisotry.QueriesRepositories;
using MediatR;

namespace Configuration.Application.Queries.ListNotification
{
    public class ListNotificationHandler : IRequestHandler<ListNotification, string>
    {
        private IAlertClassificationRepository _alertClassificationRepository;
        public ListNotificationHandler(IAlertClassificationRepository alertClassificationRepository)
        {
            _alertClassificationRepository = alertClassificationRepository;
        }

        public Task<string> Handle(ListNotification request, CancellationToken cancellationToken)
        {
            _alertClassificationRepository.List(request.isEnglish);

            return Task.FromResult("");
            // throw new NotImplementedException();
        }
    }
}
