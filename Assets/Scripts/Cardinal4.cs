using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cardinal4
{
    public enum Direction{
        EAST = 0,
        NORTH = 1,
        WEST = 2,
        SOUTH = 3,
        ZERO_VECTOR = -1
    }
    
    public static Direction Vector2ToDirection(Vector2 vector2){
        if (vector2 == new Vector2(0,0)) return Direction.ZERO_VECTOR;
        Vector2 vector2Normalized = vector2.normalized;
        float angle = Mathf.Atan2(vector2Normalized.y,vector2Normalized.x);
        if(angle < 0 ) angle += (Mathf.PI * 2);
        angle *= 2;
        angle /= Mathf.PI;
        int halfQuarter = Mathf.RoundToInt(angle);
        return (Direction) halfQuarter;
    }

    public static Vector2 DirectionToVector2 (Direction direction, float magnitude){
        if(direction == Direction.ZERO_VECTOR) return new Vector2(0,0);
        float angle = (float) direction / 2;
        angle *= Mathf.PI;
        Vector2 result = new Vector2(Mathf.Round(Mathf.Cos(angle)), Mathf.Round(Mathf.Sin(angle))).normalized;
        result *= magnitude;
        return result;
    }

    public static Direction OppositeDirection(Direction direction){
        return (Direction) ((((int) direction) + 2) % 4);
    }
}
