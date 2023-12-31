public class PlayerData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public bool IsMaster { get; set; }
    public int Color { get; set; }
    public bool IsReady { get; set; } = false;

    public PlayerData(Photon.Realtime.Player _player)
    {
        Name = _player.NickName;
        Id = _player.ActorNumber;
        if (_player.CustomProperties != null)
        {
            if (_player.CustomProperties.ContainsKey("IsReady"))
            {
                IsReady = (bool)_player.CustomProperties["IsReady"] ? (bool)_player.CustomProperties["IsReady"] : false;
            }
            if (_player.CustomProperties.ContainsKey("Color"))
            {
                Color = (int)_player.CustomProperties["Color"];
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