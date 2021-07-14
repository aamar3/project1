using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lobby : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    private const string HostAddressKey = "HostAddress";
    private ELobbyType lobbyType = ELobbyType.k_ELobbyTypeFriendsOnly;


    private void Start()
    {
        // if steam is not started
        if (!SteamManager.Initialized)
            return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // ask steam to make a lobby
    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(lobbyType, networkManager.maxConnections);
    }

    // callback for when steam lobby has been created
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("Lobby not created!!!");
            return;
        }

        networkManager.StartHost();

        // set host address
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    // callback for when client requests to join lobby
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    // callback for when client joins the lobby
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active)
            return;

        // get host address from lobby data callback
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
    }
}
