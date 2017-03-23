namespace PawPadIO.Hub.Api.Models
{
    public class UserGlobalPermission
    {
        public User User { get; set; }
        public GlobalOperation GlobalOperation { get; set; }
        public GlobalPermissionLevel PermissionLevel { get; set; }
    }

    public class GroupGlobalPermission
    {
        public Group Group { get; set; }
        public GlobalOperation GlobalOperation { get; set; }
        public GlobalPermissionLevel PermissionLevel { get; set; }
    }
}