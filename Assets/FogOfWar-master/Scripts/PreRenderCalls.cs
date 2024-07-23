using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRenderCalls : MonoBehaviour
{
    public Fog _Fog;

    void OnPreRender()
    {
        if (_Fog == null) return;
        // FOG CALL
        _Fog.SetCookie();
    }
}
