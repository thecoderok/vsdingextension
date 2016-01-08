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

Version 1.9:
* Fixed defect #8: VS 2015 stops working when looking at Test Manager Window


Version 1.8:
* Users now will be able to select their own sounds (Issue #5)
* There will be a different sound when any of the tests have failed (Issue #7)
* There is a separate option to play sound only when tests failed (thanks to https://github.com/sboulema for pull request).
* Notification in tray will have different icon when tests failed

![Custom sounds](https://cloud.githubusercontent.com/assets/3173477/12151990/c87702c0-b466-11e5-82eb-7602c430ae7c.png)


Version 1.6:
* Fixed compatibility issues with Visual studio 2012

Version 1.5:
* Added Task bar notifications. (https://github.com/thecoderok/vsdingextension/issues/1)
	It can be disabled in Tools->Options->Ding->Show tray notifications

![TaskBar notification](https://cloud.githubusercontent.com/assets/3173477/8140297/3633fb52-110f-11e5-8e53-4fcad670bd82.PNG)


Version 1.4:
* Added options dialog (Tools->Options->Ding). Now it is possible to enable/disable what sounds to play. Thanks to https://github.com/dannoh
* Added option to play sounds only if Visual Studio in background. Thanks to https://github.com/dannoh
* Support for Visual studio 2015

![options](https://cloud.githubusercontent.com/assets/3173477/8140335/89a7e618-110f-11e5-94e2-d626fefb5680.png)
