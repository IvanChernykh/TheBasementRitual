using UnityEngine;

public class SetMaterialAction : EventAction {
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Material material;
    public override void ExecuteAction() {
        if (targetObject != null && material != null) {
            if (targetObject.TryGetComponent<Renderer>(out var renderer)) {
                renderer.material = material;
            }
        }
    }
}
