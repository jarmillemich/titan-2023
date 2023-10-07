using System;
using Godot;


public class HexPoint : Godot.Object, IEquatable<HexPoint> {
    public HexPoint() : this(0, 0, 0) { }

    //https://www.redblobgames.com/grids/hexagons/
    public HexPoint(int q, int r, int s) {
        if (q + r + s != 0)
        {
            throw new InvalidOperationException("Invalid Coordinates");
        }
        
        _Q = q;
        _R = r;
        _S = s;
    }

    private readonly int _Q, _R, _S;

    public int Q => _Q;
    public int R => _R;
    public int S => _S;

    public static readonly HexPoint[] Directions = new HexPoint[] {
        new HexPoint(1, 0, -1),
        new HexPoint(1, -1, 0),
        new HexPoint(0, -1, 1),
        new HexPoint(-1, 0, 1),
        new HexPoint(-1, 1, 0),
        new HexPoint(0, 1, -1),
    };

    /// <summary>
    /// Convert from hex space to cartesian space
    /// </summary>
    public Vector2 ToVector2(float sideLength) {

        var col = _Q + (_R - (_R & 1)) / 2;
        var row = _R;
        
        float dx = 2 * sideLength * (float)Math.Sin(Math.PI / 3);
		float dy = 3 * sideLength / 2;

        float xOfs = (row & 1) != 0 ? 0.5f : 0f;
        var x = (col + xOfs) * dx;
        var y = row * dy;
        return new Vector2(x, y);
    }

    public static HexPoint operator+(HexPoint a, HexPoint b) {
        return new HexPoint(a._Q + b._Q, a._R + b._R, a._S + b._S);
    }
    
    public static HexPoint operator-(HexPoint a, HexPoint b) {
        return new HexPoint(a._Q - b._Q, a._R - b._R, a._S - b._S);
    }

    public static HexPoint operator*(HexPoint a, int b) {
        return new HexPoint(a._Q * b, a._R * b, a._S * b);
    }

    // An absolutely improper ordering
    public static bool operator<(HexPoint a, HexPoint b) {
        return a._Q < b._Q || a._R < b._R || a._S < b._S;
    }

    public static bool operator>(HexPoint a, HexPoint b) {
        return a._Q > b._Q || a._R > b._R || a._S > b._S;
    }

    public static bool operator==(HexPoint a, HexPoint b) {
        return a._Q == b._Q && a._R == b._R && a._S == b._S;
    }

    public static bool operator!=(HexPoint a, HexPoint b) {
        return !(a == b);
    }

    /// <summary>
    /// Gets the number of hexes between this and another point
    /// </summary>
    public int DistanceTo(HexPoint other) {
        var vec = this - other;
        return (Math.Abs(vec._Q) + Math.Abs(vec._R) + Math.Abs(vec._S)) / 2;
    } 

    bool IEquatable<HexPoint>.Equals(HexPoint other) {
        return this == other;
    }

    public override bool Equals(object obj) {
        if (obj is HexPoint other) {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode() {
        return _Q.GetHashCode() ^ _R.GetHashCode() ^ _S.GetHashCode();
    }

    public override string ToString()
    {
        return $"Hex<{_Q}, {_R}, {_S}>";
    }

    /// <summary>
    /// Convert to a long for use as a key in a dictionary
    /// </summary>
    public static implicit operator long(HexPoint point) {
        long ret = ((long)(uint)point._Q << 32) | ((long)(uint)point._R);
        //GD.Print($"{point._Q:x} | {point._R:x} => {ret:x}");
        return ret;
    }

    /// <summary>
    /// Convert from a long back to a HexPoint
    /// </summary>
    public static explicit operator HexPoint(long point) {
        var q = (int)(point >> 32);
        var r = (int)(point & 0xFFFFFFFF);
        var s = -q - r;
        return new HexPoint(q, r, s);
    }

}