using UnityEngine;

public class Vector
{
    public Vector3 Multiple(Vector3 a, Vector3 b)//’è‹`‚³‚ê‚Ä‚È‚¢Vector3“¯m‚ÌŠ|‚¯Z
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Vector3 CoordinatetoOne(Vector3 a)//À•W‚Ìâ‘Î’l‚ğ‚·‚×‚Ä‚P‚É‚·‚é
    {
        return new Vector3(a.x / Mathf.Abs(a.x), a.y / Mathf.Abs(a.y), a.z / Mathf.Abs(a.z));
    }
}
