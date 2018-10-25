using UnityEngine;
using System.IO;

public class GameDataWritter 
{
    BinaryWriter writer;

    // to facilate for us reading and writting difrent types
    public GameDataWritter(BinaryWriter writer)
    {
        this.writer = writer;
    }

    public void Write(float value)
    {
        writer.Write(value);
    }

    public void Write(int value)
    {
        writer.Write(value);
    }
    
    public void Write(Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    public void Write(Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public void Write(Color value)
    {
        writer.Write(value.r);
        writer.Write(value.g);
        writer.Write(value.b);
        writer.Write(value.a);
    }

}
