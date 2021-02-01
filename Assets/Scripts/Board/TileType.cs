public class TileType
{
    public bool IsAccessible { get { return _isAccessible; } }
    private bool _isAccessible;
    public bool BlocksLineOfSight { get { return _blocksLineOfSight; } }
    private bool _blocksLineOfSight;

    /// <summary>
    /// Constructor
    /// It currently ignores the type of tile, will need further implementation
    /// </summary>
    /// <param name="associatedTileInTilemap">The associated tile</param>
    public TileType(UnityEngine.Tilemaps.Tile associatedTileInTilemap)
    {
        _isAccessible = associatedTileInTilemap != null;
        _blocksLineOfSight = false;
    }

    public override string ToString()
    {
        return $"isAccessible : {IsAccessible} blocksLineOfSight : {BlocksLineOfSight}";
    }
}
