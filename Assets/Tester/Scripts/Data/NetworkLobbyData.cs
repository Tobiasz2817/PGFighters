using Unity.Services.Lobbies.Models;
using Utilities;


namespace LobbyData
{
    public class NetworkLobbyData : SingletonPersistent<NetworkLobbyData>
    {
        public int targetCountPlayers;
        public Lobby currentLobby;
    }

}
