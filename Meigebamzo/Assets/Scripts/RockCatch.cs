using UnityEngine;
using UnityEngine.Events;

public class RockCatch : MonoBehaviour
{
    [SerializeField] Sprite _catchedBallSprite;
    public UnityEvent OnBallCatched;

    public void CatchBall()
    {
        GetComponent<SpriteRenderer>().sprite = _catchedBallSprite;
        OnBallCatched.Invoke();
    }
}
