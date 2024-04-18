using UnityEngine;

public class SoundManager : MonoBehaviour
{
   [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource SFXSource;

   [Header("CLips")]

   public AudioClip background;
   public AudioClip ButtonHover;
   public AudioClip ButtonPress;

   private void Start()
   {
    musicSource.clip = background;
    musicSource.Play();

   }

   public void PlaySFX(AudioClip clip)
   {
     SFXSource.PlayOneShot(clip);
   }
}
