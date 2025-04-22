using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase
{
  protected ManagerBase()
  {
    if (!GameManager.Instance.CanInstantiate())
    {
      throw new InvalidOperationException("Instances of IDataPool implementations can only be created by GameDataPool.");
    }
  }
}
