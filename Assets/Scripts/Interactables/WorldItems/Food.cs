
public class Food : Interactable {
    private void Start() {
        interactMessage = "Eat";
    }
    protected override void Interact() {
        Destroy(gameObject);
    }
}
