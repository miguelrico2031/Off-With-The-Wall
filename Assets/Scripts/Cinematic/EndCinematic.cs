using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCinematic : MonoBehaviour
{
    [SerializeField] private Animator _wallAnim, _fadeOutAnim;
    [SerializeField] private GameObject _brokenHouses, _flags;
    [SerializeField] private ParticleSystem _wallParticles;
    
    public void StartCinematic()
    {
        StartCoroutine(Cinematic());
    }

    IEnumerator Cinematic()
    {
        var ai = GameManager.Instance.Get<IAIService>();
        AudioManager.Instance.ChangeMusic("Final");
        ai.TargetWall();
        yield return MoveCamera();
        yield return new WaitForSeconds(7f);

        _wallAnim.Play("TearDownWall");
        
        yield return new WaitForSeconds(4f);
        _wallParticles.Play();
        yield return new WaitForSeconds(1f);
        
        yield return ai.TargetHouses();
        
        yield return new WaitForSeconds(20f);
        
        _brokenHouses.SetActive(true);
        _flags.SetActive(true);
        
        AudioManager.Instance.FadeOutGameplayMusic(5f);
        yield return new WaitForSeconds(4f);
        
        _fadeOutAnim.Play("FadeOut");

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Credits");

    }

    
    
    
    IEnumerator MoveCamera()
    {
        var ct = Camera.main.transform;
        Vector3 start = ct.position;
        Vector3 target = ct.position;
        target.x = 4.4f;
        float t = 0f;
        while (Vector3.Distance(ct.position, target) > .01f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            ct.position = Vector3.Lerp(start, target, t);
            t += Time.deltaTime;
        }
    }
}
