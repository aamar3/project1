using Mirror;
using UnityEngine;
using Steamworks;
using UnityEngine.InputSystem;

public class Lobby : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private const string HostAddressKey = "HostAddress";
   // [SyncVar] bool isHosting = false;
    private void Start()
    {
         DoSteamStuff();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)// && !isHosting)
        {
            //WisHosting = true;
            HostLobby();
        }
    }

    private void DoSteamStuff()
    {
        
     if (!SteamManager.Initialized) { return; }

     lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
     gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
     lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
     return;
     
    }

    
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<LobbyEnter_t> lobbyEntered;
    private ELobbyType lobbyType = ELobbyType.k_ELobbyTypeFriendsOnly;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(lobbyType, networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if(NetworkServer.active) { return; }

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
    }
    
}
