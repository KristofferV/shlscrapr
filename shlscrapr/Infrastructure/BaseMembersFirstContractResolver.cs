using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace shlscrapr.Infrastructure
{
    public class BaseMembersFirstContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var members = GetSerializableMembers(type);
            if (members == null)
            {
                throw new JsonSerializationException("Null collection of serializable members returned.");
            }

            return members.Select(member => CreateProperty(member, memberSerialization))
                          .Where(x => x != null)
                          .OrderBy(p => GetTypeDepth(p.DeclaringType))
                          .ToList();
        }

        private static int GetTypeDepth(Type type)
        {
            var depth = 0;
            while ((type = type.BaseType) != null)
                depth++;

            return depth;
        }
    }
}
