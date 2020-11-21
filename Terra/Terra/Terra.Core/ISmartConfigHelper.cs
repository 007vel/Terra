using System;
using EspTouchMultiPlatformLIbrary;

namespace Terra.Core
{
	public interface ISmartConfigHelper
	{
		ISmartConfigTask CreatePlatformTask();
	}
}
