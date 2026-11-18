public interface IInteractable
{
    void Interact();
    string ObjectName { get; }

    // 추후에 필요하면 추가적인 메서드나 속성 추가
}