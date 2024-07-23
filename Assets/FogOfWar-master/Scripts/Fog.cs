using System;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Material CookieCreator;
    public Material CookieBlur;
    public Transform[] Centers;
    public Transform[] AnotherPlayers;
    public int CastResolution = 32;
    public int TextureResolution = 128;
    public float CastPointHeight = 1.5f;

    [Range(0, 12)]
    public int PastTexCount = 12;
    public float Radius = 3;
    public float RadiusUpper = 3;
    public bool KeepUnfoged;

    private RenderTexture cookieMask, cookieBlurred, cookieMask_i;
    private Projector projector;
    private RaycastHit hit;
    private float[] data;
    private float projectorSize;
    private float d_angle;
    private RenderTexture[] pastMasks;
    private int c_pastMask = 0;

    private bool[] playerVisible;

    void Start()
    {
        cookieMask = new RenderTexture(TextureResolution, TextureResolution, 0);
        cookieBlurred = new RenderTexture(TextureResolution, TextureResolution, 0);
        cookieMask_i = new RenderTexture(TextureResolution, TextureResolution, 0);

        pastMasks = new RenderTexture[PastTexCount];
        for (int i = 0; i < PastTexCount; i++)
        {
            pastMasks[i] = new RenderTexture(TextureResolution / 2, TextureResolution / 2, 0);
            CookieBlur.SetTexture("_PastTex" + i.ToString(), pastMasks[i]);
        }

        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", cookieBlurred);
        projectorSize = projector.orthographicSize * 2;

        CookieCreator.SetTexture("_RecurringCookie", cookieMask_i);
        CookieCreator.SetInt("_KeepUnfoged", (KeepUnfoged) ? (1) : (0));

        data = new float[CastResolution * 2 + 3];
        data[0] = CastResolution;
        d_angle = 360 * Mathf.Deg2Rad / CastResolution;

        playerVisible = new bool[AnotherPlayers.Length];
    }

    public void SetCookie()
    {
        CookieBlur.SetInt("_PastTexCount", PastTexCount);

        for (int c = 0; c < Centers.Length; c++)
        {
            Vector3 castPoint = Centers[c].position;
            castPoint.y = CastPointHeight;
            for (int i = 0; i < CastResolution; i++)
            {
                Vector3 dir = Vector3.one;
                dir.x *= Mathf.Cos(d_angle * i);
                dir.z *= Mathf.Sin(d_angle * i);
                dir.y = 0;
                if (Physics.Raycast(castPoint, dir, out hit, Radius))
                {
                    data[3 + i * 2] = (hit.point.x - transform.position.x) / projectorSize + 0.5f;
                    data[4 + i * 2] = (hit.point.z - transform.position.z) / projectorSize + 0.5f;
                }
                else
                {
                    Vector3 point = dir.normalized * Radius + Centers[c].transform.position;
                    data[3 + i * 2] = (point.x - transform.position.x) / projectorSize + 0.5f;
                    data[4 + i * 2] = (point.z - transform.position.z) / projectorSize + 0.5f;
                }
            }
            data[1] = (castPoint.x - transform.position.x) / projectorSize + 0.5f;
            data[2] = (castPoint.z - transform.position.z) / projectorSize + 0.5f;
            CookieCreator.SetFloatArray("_Data", data);
            CookieCreator.SetInt("_Index", c);
            Graphics.Blit(null, cookieMask, CookieCreator);
            Graphics.Blit(cookieMask, cookieMask_i);
        }

        Graphics.Blit(cookieMask, cookieBlurred, CookieBlur);
        Graphics.Blit(cookieBlurred, pastMasks[c_pastMask++]);
        if (c_pastMask >= PastTexCount)
        {
            c_pastMask = 0;
        }

        UpdatePlayerVisibility();
    }

    void UpdatePlayerVisibility()
    {
        for (int p = 0; p < AnotherPlayers.Length; p++)
        {
            bool isVisible = false;
            for (int c = 0; c < Centers.Length; c++)
            {
                Vector3 direction = AnotherPlayers[p].position - Centers[c].position;
                direction.y = 0; // Ignore vertical difference for simplicity
                if (Physics.Raycast(Centers[c].position + Vector3.up * CastPointHeight, direction, out hit, Radius + RadiusUpper))
                {
                    if (hit.transform == AnotherPlayers[p])
                    {
                        isVisible = true;
                        break;
                    }
                }
            }
            playerVisible[p] = isVisible;
        }
    }

    void LateUpdate()
    {
        ApplyVisibility();
    }

    void ApplyVisibility()
    {
        for (int p = 0; p < AnotherPlayers.Length; p++)
        {
            Renderer[] renderers = AnotherPlayers[p].GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = playerVisible[p];
            }
        }
    }
}
