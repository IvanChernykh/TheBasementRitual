using System;
using UnityEngine;

[Serializable]
public class PlayerData {
    public float[] playerPosition;
    public float[] playerRotation;

    public PlayerData() {
        PlayerController player = PlayerController.Instance;
        Vector3 position = player.transform.position;
        Vector3 angles = player.transform.rotation.eulerAngles;

        playerPosition = new float[] { position.x, position.y, position.z };
        playerRotation = new float[] { angles.x, angles.y, angles.z };
    }
}
