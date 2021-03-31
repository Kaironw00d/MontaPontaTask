using System;
using System.Collections.Generic;

public interface ICollectiblesProvider
{
    event Action OnListChanged;
    List<ICollectable> Get();
}