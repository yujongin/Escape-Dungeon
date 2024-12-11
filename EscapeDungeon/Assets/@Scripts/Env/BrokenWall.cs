using DG.Tweening;
using TMPro;
using UnityEngine;
public class BrokenWall : MonoBehaviour
{
    int hitCount = 0;
    public Mesh BrokenWallMesh;

    public TMP_Text WallText;

    private void Start()
    {
        WallText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);

        hitCount++;
        transform.DOShakePosition(1, 0.1f);
        GetComponent<AudioSource>().Play();
        if (hitCount > 2 && hitCount < 4)
        {
            GetComponent<MeshFilter>().mesh = BrokenWallMesh;
        }
        else if (hitCount >= 4)
        {
            transform.DOMoveY(-4.2f, 1f);
            Managers.Game.isStart = true;
            return;
        }

    }
}
