using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
	#region 내부 변수
	private AudioSource _audioSource;
	private Weapon _weapon;
	private string _soundTypeName;
	#endregion

	#region 외부 호출 함수
	public void Init(Weapon waepon)
	{
		_weapon = waepon;
		_audioSource = GetComponent<AudioSource>();
		_audioSource.playOnAwake = false;
		_soundTypeName = _weapon.WeaponData.WeaponType.ToString();
	}

	public void PlayFireSound()
	{
		if (SoundManager.Instance == null || _weapon.WeaponData.WeaponType == WeaponItemDataSO.EWeaponType.Melee)
		{
			return;
		}

		string name = $"{_soundTypeName}_Fire";
		int count = 4;

		SoundManager.Instance.PlayObjectRandomSFX(_audioSource, name, count, 1f, 30f);
	}

	public void PlayMeleeSound()
	{
        if (SoundManager.Instance == null || _weapon.WeaponData.WeaponType != WeaponItemDataSO.EWeaponType.Melee)
        {
            return;
        }

        string name = $"{_soundTypeName}_Attack";
        int count = 3;

        SoundManager.Instance.PlayObjectRandomSFX(_audioSource, name, count, 1f, 15f);
    }

	public void PlayReloadStartSound()
	{
        if (SoundManager.Instance == null || _weapon.WeaponData.WeaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            return;
        }

        string name = $"{_soundTypeName}_ReloadStart";

		SoundManager.Instance.PlayObjectSFX(_audioSource, name, 1f, 7f);
    }

	public void PlayReloadEndSound()
	{
        if (SoundManager.Instance == null || _weapon.WeaponData.WeaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            return;
        }

        string name = $"{_soundTypeName}_ReloadEnd";

        SoundManager.Instance.PlayObjectSFX(_audioSource, name, 1f, 7f);
    }

	public void PlayAmmoEmptySound()
	{
        if (SoundManager.Instance == null || _weapon.WeaponData.WeaponType == WeaponItemDataSO.EWeaponType.Melee)
        {
            return;
        }

        SoundManager.Instance.PlayObjectSFX(_audioSource, "AmmoEmpty", 1f, 7f);
    }
    #endregion
}
