﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonEquipableItem : Item
{
    public abstract void OnPickUp();
}