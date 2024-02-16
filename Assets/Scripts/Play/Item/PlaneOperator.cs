using UnityEngine;

// plane movement & drop vaccine
public class Plane : PoolAble
{
    private Vector3 dropPos;
    private Vector3 direction = new Vector3(-1, 0, 0);
    private bool alreadyDrop = false;

    private void Update()
    {
        transform.Translate(direction * StaticVars.PLANE_SPEED * Time.deltaTime);

        if (!alreadyDrop && transform.position.x < dropPos.x)
        {
            DropVaccine();
            alreadyDrop = true;
        }

        if(transform.position.x < -20.0f)
        {
            alreadyDrop = false;
            ReleaseObject();
        }
    }

    public void InitiateDrop(int _index)
    {
        AudioManager.Instance.PlayEffect(EffectAudioType.PLANE);
        transform.position = new Vector3(25.0f, NetworkManager.Instance.PlaySceneManager.RandomDropPos[_index].y, 0f);
        dropPos = NetworkManager.Instance.PlaySceneManager.RandomDropPos[_index];
    }

    // vaccine pool get
    private void DropVaccine()
    {
        var vaccine = ObjectPoolManager.Instance.GetObject("Vaccine");
        AudioManager.Instance.PlayEffect(EffectAudioType.DROP);
        vaccine.transform.position = gameObject.transform.position;
    }
}
