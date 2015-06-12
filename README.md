Visual Studio Ding extension

====================================================

This small extension will play notification sounds when following events occur:
- Build Complete
- Entering debugger mode (breakpoint hit, etc)
- Unit tests finished to run

Ability to toggle when the notifications sound can be found in the Visual Studio Options under 'Ding'

Useful when working with big solutions or when build/test run/hitting a breakpoint takes a lot of time and developer can be distructed on other things while he waits.

This is an open source project, join!

Twitter: @GanzhaVitalii

Version 1.5:
* Added Task bar notifications. (https://github.com/thecoderok/vsdingextension/issues/1)
	It can be disabled in Tools->Options->Ding->Show tray notifications

Version 1.4:
* Added options dialog (Tools->Options->Ding). Now it is possible to enable/disable what sounds to play. Thanks to https://github.com/dannoh
* Added option to play sounds only if Visual Studio in background. Thanks to https://github.com/dannoh
* Support for Visual studio 2015