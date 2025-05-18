using UnityEngine;

public class ElementSpellsIconDestoryer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _leftIcon;
    [SerializeField] SpriteRenderer _rightIcon;

    public void SetSpritesToDestroy(Sprite leftSprite,Sprite rightSprite)
    {
        _leftIcon.sprite = leftSprite;
        _rightIcon.sprite = rightSprite;
    }

}
