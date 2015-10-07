using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CrowdController : MonoBehaviour
{

    private string[] Animations = { "idle", "applause", "applause2", "celebration", "celebration2", "celebration3" };

    // Use this for initialization
    Animation[] AudienceMembers;

    public List<AudioClip> CrowdNoises = new List<AudioClip>();

    bool Cheering;

    public GameObject audioSource;

    FadingAudioSource fadeSounds;

    GameObject Head;

    float realVol;

    void Start()
    {
        realVol = 1;

        Head = GameObject.Find("Main Camera");

        AudienceMembers = gameObject.GetComponentsInChildren<Animation>();
        string thisAnimation = Animations[0];

        foreach (Animation anim in AudienceMembers)
        {
            LoopAnimation(thisAnimation, anim);
            anim.transform.LookAt(new Vector3(Head.transform.position.x, anim.transform.position.y, Head.transform.position.z));            
        }
    }

    void FixedUpdate()
    {
        //foreach (Animation anim in AudienceMembers)
        //{
        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {
        //        StartCoroutine(AnimationXtoY(anim, Random.Range(1, Animations.Length), 0, (Random.Range(0, 1.0f) + 3.0f)));
        //    }
        //}
    }

    public void Cheer()
    {
        if (!Cheering)
        {            
            Cheering = true;

            StartCoroutine(StartAnimation(0.5f));
        }
        
        StartCoroutine(playHappyCrowd(0));
    }

    private void LoopAnimation(string thisAnimation, Animation anim)
    {
        anim.wrapMode = WrapMode.Loop;

        anim.CrossFade(thisAnimation);

        anim[thisAnimation].time = Random.Range(0f, 3f);
    }

    IEnumerator StartAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Animation anim in AudienceMembers)
        {
            StartCoroutine(AnimationXtoY(anim, Random.Range(1, Animations.Length), 0, (Random.Range(0, 1.0f) + 3.0f)));
        } 
    }

    IEnumerator AnimationXtoY(Animation anim, int X, int Y, float Time)
    {
        LoopAnimation(Animations[X], anim);

        yield return new WaitForSeconds(Time);

        LoopAnimation(Animations[Y], anim);
    }

    IEnumerator playHappyCrowd(float time)
    {
        GameObject go = Instantiate(audioSource);
        go.transform.parent = transform;

        AudioSource source = go.GetComponent<AudioSource>();
        source.clip = CrowdNoises[Random.Range(0, CrowdNoises.Count - 1)];
        source.PlayScheduled(time);

        yield return new WaitForSeconds(4.0f);

        Cheering = false;
    }
}
