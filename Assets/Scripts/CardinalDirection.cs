using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardinalDirection
{
    public enum Direction8{
        EAST = 0,
        NORTH_EAST = 1,
        NORTH = 2,
        NORTH_WEST = 3,
        WEST = 4,
        SOUTH_WEST = 5,
        SOUTH = 6,
        SOUTH_EAST = 7,
        ZERO_VECTOR = -1
    }

    public enum Direction4{
        EAST = 0,
        NORTH = 1,
        WEST = 2,
        SOUTH = 3,
        ZERO_VECTOR = -1
    }
    
    public static Direction8 Vector2ToCardinalDirection8(Vector2 vector2){
        if (vector2 == new Vector2(0,0)) return Direction8.ZERO_VECTOR;
        Vector2 vector2Normalized = vector2.normalized;
        float angle = Mathf.Atan2(vector2Normalized.y,vector2Normalized.x);
        if(angle < 0 ) angle += (Mathf.PI * 2);
        angle *= 4;
        angle /= Mathf.PI;
        int halfQuarter = Mathf.RoundToInt(angle);
        return (Direction8) halfQuarter;
    }
    public static Direction4 Vector2ToCardinalDirection4(Vector2 vector2){
        if (vector2 == new Vector2(0,0)) return Direction4.ZERO_VECTOR;
        Vector2 vector2Normalized = vector2.normalized;
        float angle = Mathf.Atan2(vector2Normalized.y,vector2Normalized.x);
        if(angle < 0 ) angle += (Mathf.PI * 2);
        angle *= 2;
        angle /= Mathf.PI;
        int halfQuarter = Mathf.RoundToInt(angle);
        return (Direction4) halfQuarter;
    }

    public static Vector2 CardinalDirection4ToVector2(CardinalDirection.Direction4 direction, float magnitude){
        if(direction == Direction4.ZERO_VECTOR) return new Vector2(0,0);
        float angle = (float) direction / 2;
        angle *= Mathf.PI;
        Vector2 result = new Vector2(Mathf.Round(Mathf.Cos(angle)), Mathf.Round(Mathf.Sin(angle))).normalized;
        result *= magnitude;
        return result;
    }
}
