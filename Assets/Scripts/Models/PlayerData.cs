public class PlayerData
{
    public string Name { get; set; }
    public int Id { get; set; }
    public bool IsMaster { get; set; }
    public PlayerStatus Status { get; set; } = PlayerStatus.HOMES;
    public float Speed { get; set; } = 3;
    public string Code { get; set; } = "00000";
    public bool Vaccinated { get; set; } = false;
    public bool IsReady { get; set; } = false;

    public PlayerData(Photon.Realtime.Player _player)
    {
        Name = _player.NickName;
        Id = _player.ActorNumber;
        if (_player.CustomProperties != null)
        {
            if (_player.CustomProperties.ContainsKey("PlayerStatus"))
            {
                Status = (PlayerStatus)System.Enum.Parse(typeof(PlayerStatus), (string)_player.CustomProperties["PlayerStatus"]);
            }
            if (_player.CustomProperties.ContainsKey("Speed"))
            {
                Speed = (float)_player.CustomProperties["Speed"];
            }
            if (_player.CustomProperties.ContainsKey("PlayerCode"))
            {
                Code = (string)_player.CustomProperties["PlayerCode"];
            }
            if (_player.CustomProperties.ContainsKey("Vaccinated"))
            {
                Vaccinated = (bool)_player.CustomProperties["Vaccinated"];
            }
            if (_player.CustomProperties.ContainsKey("IsReady"))
            {
                IsReady = (bool)_player.CustomProperties["IsReady"];
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