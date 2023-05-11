using MediatR;

namespace NewScottApp.Application.Commands.ForgetPassword
{
    public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, string>
    {
        public Task<string> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
