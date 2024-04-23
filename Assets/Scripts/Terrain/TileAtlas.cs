using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    public TileClass dirt;
    public TileClass badDirt;

    public TileClass coal;
    public TileClass plastic;
    public TileClass mushroom;
    public TileClass steel;
    public TileClass trinitite;

    public TileClass wood;
    public TileClass leaves;

    public TileClass pole;
    public TileClass sign;

    public TileClass car_bottom;
    public TileClass car_front;
    public TileClass car_front_flip;
    public TileClass car_side;
    public TileClass car_tire;
    public TileClass car_top;
    public TileClass car_top_flip;

    public TileClass rock;
}
