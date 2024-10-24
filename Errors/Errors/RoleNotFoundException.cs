namespace API.Errors
{
    public class RoleNotFoundException :CustomException
    {
        public RoleNotFoundException(string roleName)
       : base($"El rol '{roleName}' no existe en el sistema.", "ROLE_NOT_FOUND", 400)
        {
        }
    }
}
