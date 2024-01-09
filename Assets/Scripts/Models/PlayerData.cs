public class PlayerData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsLocal { get; set; }
    public bool IsMaster { get; set; }
    public int Color { get; set; }
    public bool IsReady { get; set; } = false;

    public PlayerData(Photon.Realtime.Player _player)
    {
        Id = _player.ActorNumber;
        Name = _player.NickName;
        IsLocal = _player.IsLocal;
        if (_player.CustomProperties != null)
        {
            if (_player.CustomProperties.ContainsKey(StaticCodes.PHOTON_PROP_ISREADY))
            {
                IsReady = (bool)_player.CustomProperties[StaticCodes.PHOTON_PROP_ISREADY] ? (bool)_player.CustomProperties[StaticCodes.PHOTON_PROP_ISREADY] : false;
            }
            if (_player.CustomProperties.ContainsKey(StaticCodes.PHOTON_PROP_COLOR))
            {
                Color = (int)_player.CustomProperties[StaticCodes.PHOTON_PROP_COLOR];
            }
        }
    }
}

public enum PlayerStatus
{
    HOMES,
    COLLOC,
    INFECT
}