using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
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
