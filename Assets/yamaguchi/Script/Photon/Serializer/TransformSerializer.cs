using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSerializer : MonoBehaviour
{
    public static void Register()
    {
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Transform), (byte)'C', SerializeTransform, DeserializeColor);
    }

    private static byte[] SerializeTransform(object i_customobject)
    {
        Color color = (Color)i_customobject;

        var bytes = new byte[4 * sizeof(float)];
        int index = 0;
        ExitGames.Client.Photon.Protocol.Serialize(color.r, bytes, ref index);
        ExitGames.Client.Photon.Protocol.Serialize(color.g, bytes, ref index);
        ExitGames.Client.Photon.Protocol.Serialize(color.b, bytes, ref index);
        ExitGames.Client.Photon.Protocol.Serialize(color.a, bytes, ref index);

        return bytes;
    }

    private static object DeserializeColor(byte[] i_bytes)
    {
        var color = new Color();
        int index = 0;
        ExitGames.Client.Photon.Protocol.Deserialize(out color.r, i_bytes, ref index);
        ExitGames.Client.Photon.Protocol.Deserialize(out color.g, i_bytes, ref index);
        ExitGames.Client.Photon.Protocol.Deserialize(out color.b, i_bytes, ref index);
        ExitGames.Client.Photon.Protocol.Deserialize(out color.a, i_bytes, ref index);

        return color;
    }

}
