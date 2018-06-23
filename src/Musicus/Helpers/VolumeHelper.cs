using System;
using System.Runtime.InteropServices;
using Vannatech.CoreAudio.Enumerations;
using Vannatech.CoreAudio.Interfaces;

namespace Musicus.Helpers
{
	public static class VolumeHelper
	{
		public static void SetVolume(float newVolume)
		{
			IAudioEndpointVolume masterVol = null;
			try
			{
				masterVol = GetMasterVolumeObject();
				if (masterVol == null) return;

				masterVol.SetMasterVolumeLevelScalar(newVolume / 100, Guid.Empty);
			}
			finally
			{
				if (masterVol != null) Marshal.ReleaseComObject(masterVol);
			}
		}

		public static float GetVolume()
		{
			IAudioEndpointVolume masterVol = null;
			try
			{
				masterVol = GetMasterVolumeObject();
				if (masterVol == null) return -1;

				masterVol.GetMasterVolumeLevelScalar(out var volumeLevel);

				return volumeLevel * 100;
			}
			finally
			{
				if (masterVol != null) Marshal.ReleaseComObject(masterVol);
			}
		}

		private static IAudioEndpointVolume GetMasterVolumeObject()
		{
			IMMDeviceEnumerator deviceEnumerator = null;
			IMMDevice speakers = null;
			try
			{
				deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
				deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

				Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
				speakers.Activate(IID_IAudioEndpointVolume, 0, IntPtr.Zero, out var obj);
				var masterVol = (IAudioEndpointVolume)obj;

				return masterVol;
			}
			finally
			{
				if (speakers != null) Marshal.ReleaseComObject(speakers);
				if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
			}
		}
	}

	[ComImport]
	[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
	internal class MMDeviceEnumerator
	{
	}
}
