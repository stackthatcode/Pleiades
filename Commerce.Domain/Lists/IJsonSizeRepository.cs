using System;
using System.Collections.Generic;
using Commerce.Application.Lists.Entities;

namespace Commerce.Application.Lists
{
    public interface IJsonSizeRepository
    {
        List<JsonSizeGroup> RetrieveAll(bool includeDefault);
        JsonSizeGroup RetrieveByGroup(int groupId);
        Func<JsonSizeGroup> Insert(JsonSizeGroup SizeGroup);
        void Update(JsonSizeGroup SizeGroup);
        void DeleteSoft(JsonSizeGroup SizeGroup);

        Func<JsonSize> Insert(JsonSize Size);
        void Update(JsonSize Size);
        void DeleteSoft(JsonSize Size);
    }
}
