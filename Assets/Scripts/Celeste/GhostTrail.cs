using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    public PlayerController player;
    public Transform ghostsParent;
    public Color trailColor;
    public Color fadeColor;
    public float ghostInterval;
    public float fadeTime;

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();
        for (int i = 0; i < ghostsParent.childCount; ++i)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            SpriteRenderer childSR = currentGhost.GetComponent<SpriteRenderer>();
            s.AppendCallback(()=> currentGhost.position = PlayerHealthController.instance.transform.position);
            s.AppendCallback(() => childSR.flipX = PlayerHealthController.instance.transform.localScale.x!=1f);
            s.AppendCallback(()=> childSR.sprite = player.GetCurSprite().sprite);
            s.Append(childSR.material.DOColor(trailColor, 0));
            s.AppendCallback(() => FadeSprite(childSR.material));
            s.AppendInterval(ghostInterval);
        }
    }

    public void FadeSprite(Material CurMat)
    {
        CurMat.DOKill();
        CurMat.DOColor(fadeColor, fadeTime);
    }
}
