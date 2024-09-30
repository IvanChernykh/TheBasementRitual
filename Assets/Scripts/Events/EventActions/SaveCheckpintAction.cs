
public class SaveCheckpintAction : EventAction {
    public override void ExecuteEvent() {
        SaveSystem.SaveGame();
    }
}
