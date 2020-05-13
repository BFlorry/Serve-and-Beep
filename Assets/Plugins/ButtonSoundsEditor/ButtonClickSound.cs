using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Plugins.ButtonSoundsEditor
{
    public class ButtonClickSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, ISubmitHandler
    {
        public AudioSource AudioSource;
        public AudioClip ClickSound;
        public AudioClip SelectSound;
        public AudioClip DeselectSound;

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClickSound();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySelectSound();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayDeSelectSound();
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlaySelectSound();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            PlayClickSound();
        }

        private void PlayClickSound()
        {
            AudioSource.PlayOneShot(ClickSound);
        }

        private void PlaySelectSound()
        {
            AudioSource.PlayOneShot(SelectSound);
        }

        private void PlayDeSelectSound()
        {
            //AudioSource.PlayOneShot(DeselectSound);
        }
    }

}
