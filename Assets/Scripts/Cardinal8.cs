using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cardinal8
{
    public enum Direction{
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

    public static Direction Vector2ToDirection(Vector2 vector2){
        if (vector2 == new Vector2(0,0)) return Direction.ZERO_VECTOR;
        Vector2 vector2Normalized = vector2.normalized;
        float angle = Mathf.Atan2(vector2Normalized.y,vector2Normalized.x);
        if(angle < 0 ) angle += (Mathf.PI * 2);
        angle *= 4;
        angle /= Mathf.PI;
        int halfQuarter = Mathf.RoundToInt(angle);
        return (Direction) halfQuarter;
    }
}
