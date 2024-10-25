namespace API.Errors
{
    public class UserAlreadyExistsException :CustomException
    {
        public UserAlreadyExistsException(string username)
      : base($"El usuario '{username}' ya existe en el sistema.", "USER_ALREADY_EXISTS", 400)
        {
        }
    }
}
