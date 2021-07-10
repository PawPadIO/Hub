using GraphQL.Types;
using PawPadIO.Hub.Domain.Models;

namespace PawPadIO.Hub.GraphQL.Types
{
    // TODO: Is there a better way than having to register every single GraphQL type with DI?
    public class HubUserType : ObjectGraphType<HubUser>
    {
        public HubUserType()
        {
            Name = "HubUser";
            Description = "A user registered to the PawPad.IO Hub.";
            Field(u => u.Id, nullable: false).Description("Id");
            Field(u => u.Name, nullable: true).Description("The name of the user.");
            Field(u => u.Email, nullable: true).Description("The user's email address.");
            Field(u => u.Subject, nullable: true).Description("The user's ID.");
            Field(u => u.Issuer, nullable: true).Description("The user's issuer.");
            //Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");
        }
    }
}
