using System;

using GraphQL.Utilities;

namespace PawPadIO.Hub.GraphQL
{ 
    public class Schema : global::GraphQL.Types.Schema
    {
        public Schema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<Query>();
            Mutation = provider.GetRequiredService<Mutation>();
            Subscription = provider.GetRequiredService<Subscription>();
        }
    }
}
