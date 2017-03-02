using UnityEngine;

static class InuMath
{
    #region table
    readonly static float[] s_sinTable = 
    {
        0.00000f,
        0.01745f,
        0.03490f,
        0.05234f,
        0.06976f,
        0.08716f,
        0.10453f,
        0.12187f,
        0.13917f,
        0.15643f,
        0.17365f,
        0.19081f,
        0.20791f,
        0.22495f,
        0.24192f,
        0.25882f,
        0.27564f,
        0.29237f,
        0.30902f,
        0.32557f,
        0.34202f,
        0.35837f,
        0.37461f,
        0.39073f,
        0.40674f,
        0.42262f,
        0.43837f,
        0.45399f,
        0.46947f,
        0.48481f,
        0.50000f,
        0.51504f,
        0.52992f,
        0.54464f,
        0.55919f,
        0.57358f,
        0.58779f,
        0.60182f,
        0.61566f,
        0.62932f,
        0.64279f,
        0.65606f,
        0.66913f,
        0.68200f,
        0.69466f,
        0.70711f,
        0.71934f,
        0.73135f,
        0.74314f,
        0.75471f,
        0.76604f,
        0.77715f,
        0.78801f,
        0.79864f,
        0.80902f,
        0.81915f,
        0.82904f,
        0.83867f,
        0.84805f,
        0.85717f,
        0.86603f,
        0.87462f,
        0.88295f,
        0.89101f,
        0.89879f,
        0.90631f,
        0.91355f,
        0.92050f,
        0.92718f,
        0.93358f,
        0.93969f,
        0.94552f,
        0.95106f,
        0.95630f,
        0.96126f,
        0.96593f,
        0.97030f,
        0.97437f,
        0.97815f,
        0.98163f,
        0.98481f,
        0.98769f,
        0.99027f,
        0.99255f,
        0.99452f,
        0.99619f,
        0.99756f,
        0.99863f,
        0.99939f,
        0.99985f,
        1.00000f,
    };

    readonly static float[] s_cosTable = 
    {
        1.00000f,
        0.99985f,
        0.99939f,
        0.99863f,
        0.99756f,
        0.99619f,
        0.99452f,
        0.99255f,
        0.99027f,
        0.98769f,
        0.98481f,
        0.98163f,
        0.97815f,
        0.97437f,
        0.97030f,
        0.96593f,
        0.96126f,
        0.95630f,
        0.95106f,
        0.94552f,
        0.93969f,
        0.93358f,
        0.92718f,
        0.92050f,
        0.91355f,
        0.90631f,
        0.89879f,
        0.89101f,
        0.88295f,
        0.87462f,
        0.86603f,
        0.85717f,
        0.84805f,
        0.83867f,
        0.82904f,
        0.81915f,
        0.80902f,
        0.79864f,
        0.78801f,
        0.77715f,
        0.76604f,
        0.75471f,
        0.74314f,
        0.73135f,
        0.71934f,
        0.70711f,
        0.69466f,
        0.68200f,
        0.66913f,
        0.65606f,
        0.64279f,
        0.62932f,
        0.61566f,
        0.60182f,
        0.58779f,
        0.57358f,
        0.55919f,
        0.54464f,
        0.52992f,
        0.51504f,
        0.50000f,
        0.48481f,
        0.46947f,
        0.45399f,
        0.43837f,
        0.42262f,
        0.40674f,
        0.39073f,
        0.37461f,
        0.35837f,
        0.34202f,
        0.32557f,
        0.30902f,
        0.29237f,
        0.27564f,
        0.25882f,
        0.24192f,
        0.22495f,
        0.20791f,
        0.19081f,
        0.17365f,
        0.15643f,
        0.13917f,
        0.12187f,
        0.10453f,
        0.08716f,
        0.06976f,
        0.05234f,
        0.03490f,
        0.01745f,
        0.00000f,
    };

    readonly static float[] s_tanTable = 
    {
        0.00000f,
        0.01746f,
        0.03492f,
        0.05241f,
        0.06993f,
        0.08749f,
        0.10510f,
        0.12278f,
        0.14054f,
        0.15838f,
        0.17633f,
        0.19438f,
        0.21256f,
        0.23087f,
        0.24933f,
        0.26795f,
        0.28675f,
        0.30573f,
        0.32492f,
        0.34433f,
        0.36397f,
        0.38386f,
        0.40403f,
        0.42447f,
        0.44523f,
        0.46631f,
        0.48773f,
        0.50953f,
        0.53171f,
        0.55431f,
        0.57735f,
        0.60086f,
        0.62487f,
        0.64941f,
        0.67451f,
        0.70021f,
        0.72654f,
        0.75355f,
        0.78129f,
        0.80978f,
        0.83910f,
        0.86929f,
        0.90040f,
        0.93252f,
        0.96569f,
        1.00000f,
        1.03553f,
        1.07237f,
        1.11061f,
        1.15037f,
        1.19175f,
        1.23490f,
        1.27994f,
        1.32704f,
        1.37638f,
        1.42815f,
        1.48256f,
        1.53986f,
        1.60033f,
        1.66428f,
        1.73205f,
        1.80405f,
        1.88073f,
        1.96261f,
        2.05030f,
        2.14451f,
        2.24604f,
        2.35585f,
        2.47509f,
        2.60509f,
        2.74748f,
        2.90421f,
        3.07768f,
        3.27085f,
        3.48741f,
        3.73205f,
        4.01078f,
        4.33148f,
        4.70463f,
        5.14455f,
        5.67128f,
        6.31375f,
        7.11537f,
        8.14435f,
        9.51436f,
        11.43005f,
        14.30067f,
        19.08114f,
        28.63625f,
        57.28996f,
    };
    #endregion

    public static float Cos(float _angle)
    {
        while (_angle < 0)
            _angle += 360;
        int intAngle = ((int)_angle) % 360;
        bool mirror_x = false;
        bool mirror_y = false;
        if (intAngle < 90)
        {
            mirror_x = false;
            mirror_y = false;
        }
        else if (intAngle < 180)
        {
            mirror_x = true;
            mirror_y = true;
        }
        else if (intAngle < 270)
        {
            mirror_x = true;
            mirror_y = false;
        }
        else
        {
            mirror_x = false;
            mirror_y = true;
        }
        intAngle %= 90;

        if (mirror_y)
            intAngle = 90 - intAngle;

        float v = s_cosTable[intAngle];

        if (mirror_x)
            v = -v;
        return v;
    }

    public static float Sin(float _angle)
    {
        while (_angle < 0)
            _angle += 360;
        int intAngle = ((int)_angle) % 360;
        bool mirror_x = false;
        bool mirror_y = false;
        if (intAngle < 90)
        {
            mirror_x = false;
            mirror_y = false;
        }
        else if (intAngle < 180)
        {
            mirror_x = false;
            mirror_y = true;
        }
        else if (intAngle < 270)
        {
            mirror_x = true;
            mirror_y = false;
        }
        else
        {
            mirror_x = true;
            mirror_y = true;
        }
        intAngle %= 90;

        if (mirror_y)
            intAngle = 90 - intAngle;

        float v = s_sinTable[intAngle];

        if (mirror_x)
            v = -v;
        return v;
    }

    public static float Tan(float _angle)
    {
        return s_tanTable[(int)_angle];
    }
}


public struct InuFloat
{
    public const long INT_SCALE = 2 * 1024;
    public const float FLOAT_SCALE = 2 * 1024;
    //public static readonly InuFloat ZERO = new InuFloat(0);
    public static InuFloat ZERO
    {
        get
        {
            return new InuFloat(0);
        }
    }
    public static InuFloat FromScaledValue(long _v)
    {
        InuFloat result = new InuFloat();
        result.ScaledSet(_v);
        return result;
    }

    long v;

    public long i
    {
        get
        {
            return v;
        }
    }
    public float floatValue
    {
        get
        {
            return v / FLOAT_SCALE;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return floatValue.ToString();
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public InuFloat(float _floatValue)
    {
        v = (long)(_floatValue * FLOAT_SCALE);
    }

    public void Set(float _floatValue)
    {
        v = (long)(_floatValue * FLOAT_SCALE);
    }
    public void ScaledSet(long _value)
    {
        v = _value;
    }

    public static InuFloat Abs(InuFloat f)
    {
        if (f.v >= 0)
            return f;
        InuFloat result = new InuFloat();
        result.v = -f.v;
        return result;
    }
    public static InuFloat Min(InuFloat c1, InuFloat c2)
    {
        if (c1.v <= c2.v) return c1;
        return c2;
    }
    public static InuFloat Max(InuFloat c1, InuFloat c2)
    {
        if (c1.v >= c2.v) return c1;
        return c2;
    }
    public static InuFloat Clamp(InuFloat c, InuFloat min, InuFloat max)
    {
        if (c < min) return min;
        else if (c > max) return max;
        else return c;
    }
    public static InuFloat Lerp(InuFloat start, InuFloat end, InuFloat t)
    {
        return start + (end - start) * t;
    }
    public static int Round(InuFloat f)
    {
        int baseValue = (int)f.floatValue;
        InuFloat m = f - baseValue;
        if (m > 0.5f)
        {
            return baseValue + 1;
        }
        else if (m < -0.5f)
        {
            return baseValue - 1;
        }
        else
        {
            return baseValue;
        }
    }
    public static int RoundUp(InuFloat f)
    {
        int baseValue = (int)f.floatValue;
        InuFloat m = f - baseValue;
        if (m > 0.0f)
        {
            return baseValue + 1;
        }
        else
        {
            return baseValue;
        }
    }
    public static int RoundDown(InuFloat f)
    {
        int baseValue = (int)f.floatValue;
        InuFloat m = f - baseValue;
        if (m >= 0.0f)
        {
            return baseValue;
        }
        else
        {
            return baseValue - 1;
        }
    }
    public static InuFloat operator +(InuFloat c1, InuFloat c2)
    {
        InuFloat result = new InuFloat();
        result.v = c1.v + c2.v;
        return result;
    }
    public static InuFloat operator +(InuFloat c1, float c2)
    {
        InuFloat result = new InuFloat();
        result.v = c1.v + (long)(c2 * FLOAT_SCALE);
        return result;
    }

    public static InuFloat operator -(InuFloat c1, InuFloat c2)
    {
        InuFloat result = new InuFloat();
        result.v = c1.v - c2.v;
        return result;
    }
    public static InuFloat operator -(InuFloat c1, float c2)
    {
        InuFloat result = new InuFloat();
        result.v = c1.v - (long)(c2 * FLOAT_SCALE);
        return result;
    }

    public static InuFloat operator *(InuFloat c1, InuFloat c2)
    {
        InuFloat result = new InuFloat();
        result.v = (c1.v * c2.v); // INT_SCALE;
        if (result.v < INT_SCALE && result.v > -INT_SCALE && result.v != 0)
            result.v = result.v > 0 ? 1 : -1;
        else
            result.v /= INT_SCALE;
        return result;
    }

    public static InuFloat operator *(InuFloat c1, float c2)
    {
        InuFloat result = new InuFloat();
        result.v = (int)(c1.v * c2);
        return result;
    }

    public static InuFloat operator /(InuFloat c1, InuFloat c2)
    {
        InuFloat result = new InuFloat();
        result.v = c1.v * INT_SCALE / c2.v;
        return result;
    }
    public static InuFloat operator /(InuFloat c1, float c2)
    {
        InuFloat result = new InuFloat();
        result.v = (long)(c1.v / c2);
        return result;
    }
    public static InuFloat operator -(InuFloat c1)
    {
        InuFloat result = new InuFloat();
        result.v = -c1.v;
        return result;
    }

    public static bool operator >(InuFloat c1, float c2)
    {
        return c1.v > (long)(c2 * FLOAT_SCALE);
    }
    public static bool operator >(InuFloat c1, InuFloat c2)
    {
        return c1.v > c2.v;
    }
    public static bool operator >=(InuFloat c1, float c2)
    {
        return c1.v >= (long)(c2 * FLOAT_SCALE);
    }
    public static bool operator >=(InuFloat c1, InuFloat c2)
    {
        return c1.v >= c2.v;
    }

    public static bool operator <(InuFloat c1, float c2)
    {
        return c1.v < (long)(c2 * FLOAT_SCALE);
    }
    public static bool operator <(InuFloat c1, InuFloat c2)
    {
        return c1.v < c2.v;
    }

    public static bool operator <=(InuFloat c1, float c2)
    {
        return c1.v <= (long)(c2 * FLOAT_SCALE);
    }
    public static bool operator <=(InuFloat c1, InuFloat c2)
    {
        return c1.v <= c2.v;
    }

    public static bool operator ==(InuFloat c1, InuFloat c2)
    {
        return c1.v == c2.v;
    }
    public static bool operator ==(InuFloat c1, int c2)
    {
        return c1.v == c2 * INT_SCALE;
    }
    public static bool operator !=(InuFloat c1, InuFloat c2)
    {
        return c1.v != c2.v;
    }
    public static bool operator !=(InuFloat c1, int c2)
    {
        return c1.v != c2 * INT_SCALE;
    }

    public static InuFloat Sqrt(InuFloat _sqr)
    {
        //Debug.LogError("111: " + _sqr);
        if (_sqr.i < 0)
        {            
            return InuFloat.ZERO;
        }
        if (_sqr.i == 0) return InuFloat.ZERO;
        long b = _sqr.i; // big
        long s = 0;

        if (_sqr.i > INT_SCALE * INT_SCALE)
        {
            if (b > 100)
            {
                b = 10;
                while (b * (b / INT_SCALE) < _sqr.i)
                {
                    b *= 10;
                }
            }
            //int loop = 0;
            while (b - s > 1)
            {
                long m = (b + s) / 2;
                if (m * (m / INT_SCALE) > _sqr.i)
                {
                    b = m;
                }
                else
                {
                    s = m;
                }

                //loop++;
            }
        }
        else
        {
            b *= 10;
            //long new_scale = INT_SCALE;// *INT_SCALE;
            //long sqr_i = _sqr.i;// *INT_SCALE;
            //b *= INT_SCALE;
            //s *= INT_SCALE;
            while (b - s > 1)
            {
                long m = (b + s) / 2;
                if (m * m > _sqr.i * INT_SCALE)
                {
                    b = m;
                }
                else
                {
                    s = m;
                }

                //loop++;
            }
            //b /= INT_SCALE;
            //s /= INT_SCALE;
        }
        //Debug.LogError("loop: " + loop);
        InuFloat result = new InuFloat();

        if (s == 0)
            result.ScaledSet(1);
        else
            result.ScaledSet(s);
        //Debug.LogError("222: " + result);
        return result;
    }
}

public struct InuVector2
{
    public InuFloat x;
    public InuFloat z;

    public static InuVector2 zero
    {
        get
        {
            return new InuVector2(new InuFloat(0), new InuFloat(0));
        }
    }
    public InuFloat magnitude
    {
        get
        {
            InuFloat s = (x * x) + (z * z);
            //return new InuFloat(Mathf.Sqrt(s.floatValue));
            return InuFloat.Sqrt(s);

        }
    }

    public InuFloat sqrMagnitude
    {
        get
        {
            return (x * x) + (z * z);
        }
    }

    public void Normalize()
    {
        InuFloat mag = magnitude;
        if (mag == 0)
        {
            Debug.LogError("InuVector2 Normalize Error x:" + x.i + "  z:" + z.i);
            return;
        }
        x /= mag;
        z /= mag;
    }
    public InuVector2 normalized
    {
        get
        {
            InuFloat mag = magnitude;
            InuFloat xx = x / mag;
            InuFloat zz = z / mag;

            return new InuVector2(xx, zz);
        }
    }

    public Vector3 ToUnityVector3()
    {
        return new Vector3(x.floatValue, 0, z.floatValue);
    }
    public Vector3 ToUnityVector3(float _y)
    {
        return new Vector3(x.floatValue, _y, z.floatValue);
    }

    public static InuFloat Distance(InuVector2 _c1, InuVector2 _c2)
    {
        return (_c1 - _c2).magnitude;
    }

    public static InuVector2 Lerp(InuVector2 _start, InuVector2 _end, InuFloat _t)
    {
        InuVector2 result = new InuVector2();
        result.x = InuFloat.Lerp(_start.x, _end.x, _t);
        result.z = InuFloat.Lerp(_start.z, _end.z, _t);
        return result;
    }
    public InuVector2(InuFloat _x, InuFloat _z)
    {
        x = _x;
        z = _z;
    }
    public InuVector2(float _x, float _z)
    {
        x = new InuFloat(_x);
        z = new InuFloat(_z);
    }
    public InuVector2(Vector3 _v)
    {
        x = new InuFloat(_v.x);
        z = new InuFloat(_v.z);
    }

    public static InuVector2 operator +(InuVector2 c1, InuVector2 c2)
    {
        return new InuVector2(c1.x + c2.x, c1.z + c2.z);
    }

    public static InuVector2 operator -(InuVector2 c1, InuVector2 c2)
    {
        return new InuVector2(c1.x - c2.x, c1.z - c2.z);
    }

    public static bool operator ==(InuVector2 c1, InuVector2 c2)
    {
        return c1.x == c2.x && c1.z == c2.z;
    }

    public static bool operator !=(InuVector2 c1, InuVector2 c2)
    {
        return c1.x != c2.x || c1.z != c2.z;
    }

    public static InuVector2 operator *(InuVector2 c1, InuFloat c2)
    {
        return new InuVector2(c1.x * c2, c1.z * c2);
    }

    //public void Set(InuFloat _x, InuFloat _z)
    //{
    //    x = _x;
    //    z = _z;
    //}
}