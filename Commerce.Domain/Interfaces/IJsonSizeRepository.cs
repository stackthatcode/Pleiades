using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists.Json;

namespace Commerce.Domain.Interfaces
{
    public interface IJsonSizeRepository
    {
        List<JsonSizeGroup> RetrieveAll();
        List<JsonSizeGroup> RetrieveByGroup(int groupId);
        Func<JsonSize> Insert(JsonSize JsonSize);
        Func<JsonSizeGroup> Insert(JsonSizeGroup JsonSizeGroup);
        void Update(JsonSize JsonSize);
        void Update(JsonSizeGroup JsonSizeGroup);
        void DeleteSoft(JsonSize JsonSize);
        void DeleteSoft(JsonSizeGroup JsonSizeGroup);
    }
}
