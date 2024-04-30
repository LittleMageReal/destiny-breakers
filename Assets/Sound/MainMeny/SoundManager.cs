using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager instance;
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

   [Header("CLips")]

   public AudioClip background;
   public AudioClip ButtonHover;
   public AudioClip ButtonPress;

   private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        musicSource.clip = background;
        musicSource.Play();
    }

   public void PlaySFX(AudioClip clip)
   {
     SFXSource.PlayOneShot(clip);
   }
}
