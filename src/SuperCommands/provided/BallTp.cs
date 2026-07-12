using UnityEngine;

namespace SuperCommands.provided;

public sealed class BallTp : Command
{
    public override string Name => "balltp";
    public override string Description => "Teleport a player to their ball.";
    public override string Usage => "/balltp [player]";

    public override void Execute(string[] args)
    {
        PlayerInfo player = Utils.GetPlayerByName(args[0]) ?? GameManager.LocalPlayerInfo;
        if (player.AsGolfer.OwnBall == null) throw new CommandException(this, $"Player {player.name} is not golfing.");

        Vector3 ballPos = player.AsGolfer.OwnBall.Rigidbody.position;

        player.Movement.PlayerInfo.NetworkRigidbody.CmdTeleport(ballPos, player.Movement.transform.rotation);
        Utils.SendMessage($"Teleported {player.name} to their ball!");
    }
}