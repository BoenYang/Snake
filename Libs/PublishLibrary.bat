set CurDir=%~dp0
set LibName=%1
pdb2mdb %LibName%.dll
xcopy %LibName%.dll %CurDir%..\Assets\Plugins\ManagedLib\ /y
xcopy %LibName%.dll.mdb %CurDir%..\Assets\Plugins\ManagedLib\ /y