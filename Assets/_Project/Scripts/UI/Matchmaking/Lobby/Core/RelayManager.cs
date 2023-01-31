
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> CreateRelay(int playersCount)
    {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(playersCount);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
            NetworkManager.Singleton.StartHost();
    
            return joinCode;
        }
        catch (RelayServiceException e) {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async void JoinRelay(string joinCode)
    {
        try {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData,allocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e) {
            Console.WriteLine(e);
        }
    }
}
