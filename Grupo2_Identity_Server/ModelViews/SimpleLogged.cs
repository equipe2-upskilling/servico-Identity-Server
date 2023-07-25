using Grupo2_Identity_Server.Entities;

namespace Grupo2_Identity_Server.ModelViews
{
    public class SimpleLogged
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public static SimpleLogged Build(User user)
        {
            return new SimpleLogged
            {
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
