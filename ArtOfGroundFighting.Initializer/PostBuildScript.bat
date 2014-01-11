REM %1 => $(ProjectDir)
REM %2 => $(SolutionDir)
REM %3 => $(TargetDir)

echo %1
echo %2
echo %3

REM Copies the XDT transforms into ArtOfGroundFighting.Web root
copy %1App.Debug.config %1\..\ArtOfGroundFighting.Web\Web.Debug.config
copy %1App.Azure.config %1\..\ArtOfGroundFighting.Web\Web.Azure.config

REM Copies the Config Files into Commerce.Web root
copy %1App.Debug.config %1\..\Commerce.Web\Web.Debug.config
copy %1App.Azure.config %1\..\Commerce.Web\Web.Azure.config

REM Copies the Email templates
xcopy %1EmailTemplates %1\..\Commerce.Web\EmailTemplates /Y /S
xcopy %1EmailTemplates %1\..\ArtOfGroundFighting.Web\EmailTemplates /Y /S

