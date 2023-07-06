using System;


[Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is not Point point) return false;
        return this.x == point.x && this.y == point.y;
    }
    

    public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);


    public Point GetPoint(int sideNum)
    {
        switch (sideNum)
        {
            case 0: return Up();
            case 1: return RightUp();
            case 2: return Right();
            case 3: return RightDown();
            case 4: return Down();
            case 5: return LeftDown();
            case 6: return Left();
            case 7: return LeftUp();
            default: return this;
        }
    }


    public void Abs()
    {
        x = Math.Abs(x);
        y = Math.Abs(y);
    }
    
    
    public Point Right() => new Point(x + 1, y);
    public Point Left() => new Point(x - 1, y);
    public Point Up() => new Point(x, y - 1);
    public Point Down() => new Point(x, y + 1);
    public Point RightUp() => new Point(x + 1, y - 1);
    public Point RightDown() => new Point(x + 1, y + 1);
    public Point LeftUp() => new Point(x - 1, y - 1);
    public Point LeftDown() => new Point(x - 1, y + 1);
    
    
    public override string ToString()
    {
        return "x: " + x + " y: " + y;
    }
}