using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public static class BasisAvatarFactory
{
    public static async Task LoadAvatar(BasisLocalPlayer Player, string AvatarAddress)
    {
        var data = await AddressableResourceProcess.LoadAsGameObjectsAsync(AvatarAddress, new UnityEngine.ResourceManagement.ResourceProviders.InstantiationParameters());
        List<GameObject> Gameobjects = data.Item1;
        if (Gameobjects.Count != 0)
        {
            foreach (GameObject gameObject in Gameobjects)
            {
                if (gameObject.TryGetComponent(out BasisAvatar Avatar))
                {
                    Player.Avatar = Avatar;
                    CreateLocal(Player);
                    Player.InitalizeIKCalibration(Player.AvatarDriver);
                    if(BasisScene.Instance != null)
                    {
                        BasisScene.Instance.SpawnPlayer(Player);
                    }
                }
            }
        }
    }
    public static async Task LoadAvatar(BasisRemotePlayer Player, string AvatarAddress)
    {
        var data = await AddressableResourceProcess.LoadAsGameObjectsAsync(AvatarAddress, new UnityEngine.ResourceManagement.ResourceProviders.InstantiationParameters());
        List<GameObject> Gameobjects = data.Item1;
        if (Gameobjects.Count != 0)
        {
            foreach (GameObject gameObject in Gameobjects)
            {
                if (gameObject.TryGetComponent(out BasisAvatar Avatar))
                {
                    Player.Avatar = Avatar;
                    CreateRemote(Player);
                    Player.InitalizeIKCalibration(Player.RemoteAvatarDriver);
                }
            }
        }
    }
    public static async Task LoadAvatar(BasisPlayer Player, string AvatarAddress)
    {
        if (Player.IsLocal)
        {
            await LoadAvatar((BasisLocalPlayer)Player, AvatarAddress);
        }
        else
        {
            await LoadAvatar((BasisRemotePlayer)Player, AvatarAddress);
        }
    }
    public static void CreateRemote(BasisRemotePlayer Player)
    {
        if (Player == null)
        {
            Debug.LogError("Missing RemotePlayer");
            return;
        }
        if (Player.Avatar == null)
        {
            Debug.LogError("Missing Avatar");
            return;
        }
        Player.RemoteAvatarDriver.RemoteCalibration(Player);
    }
    public static void CreateLocal(BasisLocalPlayer Player)
    {
        if (Player == null)
        {
            Debug.LogError("Missing LocalPlayer");
            return;
        }
        if (Player.Avatar == null)
        {
            Debug.LogError("Missing Avatar");
            return;
        }
        Player.AvatarDriver.InitialLocalCalibration(Player);
    }
}