using Unity.Netcode;
using Unity.Collections;
using System;

public struct ScoreEntry : INetworkSerializable, IEquatable<ScoreEntry>
{
    public FixedString32Bytes PlayerName;
    public ulong ClientId;
    public int Kills;
    public int Deaths;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref Kills);
        serializer.SerializeValue(ref Deaths);
    }

    public bool Equals(ScoreEntry other)
    {
        return ClientId == other.ClientId &&
               PlayerName.Equals(other.PlayerName) &&
               Kills == other.Kills &&
               Deaths == other.Deaths;
    }

    public override bool Equals(object obj)
    {
        return obj is ScoreEntry other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PlayerName, ClientId, Kills, Deaths);
    }
}
