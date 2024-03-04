using GameCore.GameServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Audio
{
	public sealed class AudioSfxPlayer
	{
		private readonly IAudioService _audioService = ServiceLocator.Container.GetService<AudioService>();
		private readonly AudioClip _clip;

		public AudioSfxPlayer() { }

		public AudioSfxPlayer(AudioClip clip) =>
			_clip = clip;

		public void PlayOneShot()
		{
			if (_clip == null)
				return;
			_audioService.PlaySfxClipOneShot(_clip);
		}

		public void PlayOneShot(AudioClip clip) =>
			_audioService.PlaySfxClipOneShot(clip);

		public void PlayClickButton() =>
			_audioService.PlayClickButton();

		public void PlayClickButton(ClickEvent evt) =>
			_audioService.PlayClickButton();

		public void PlaySelectButton(MouseEnterEvent evt) =>
			_audioService.PlaySelectButton();
	}
}