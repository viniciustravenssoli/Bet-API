using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet.Application.UseCases.User.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePassword request);
}
