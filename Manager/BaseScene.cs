using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class BaseScene
{ //씬 로드할 때 필요한 init, release 등이 겹쳐서 상속시켜주기 위한 부모 클래스
    public abstract void LoadResources();
    
    //public abstract void OnPadeOut();
    public abstract void Init(); //초깃값 설정용
    
    public abstract void Release(); //주소 해제용
}