using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INavigatable
{
    public void Travel(INavigatable location);
    public void Enter(BaseRoom room = null);
}
