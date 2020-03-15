using System.Linq;

using GraphQL;

namespace PawPadIO.Hub.GraphQL
{ 
    public class Schema : global::GraphQL.Types.Schema
    {
        public Schema(Query query, Mutation mutation, Subscription subscription, IDependencyResolver resolver)
        {
            Query = query.Fields.Any() ? query : null;
            Mutation = mutation.Fields.Any() ? mutation : null;
            Subscription = subscription.Fields.Any() ? subscription : null;
            DependencyResolver = resolver;
        }
    }
}
