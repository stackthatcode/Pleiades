REM %1 => $(ProjectDir)
REM %2 => $(SolutionDir)
REM %3 => $(TargetDir)

REM Copies library contents into binary output
xcopy %2\lib %3 /Y /S

REM Copies the Config Files into Commerce.Web root
copy %1web.Debug.config %1\..\Commerce.Web\Web.Debug.config
copy %1web.AzureSandbox.config %1\..\Commerce.Web\Web.AzureSandbox.config
copy %1web.Azure.config %1\..\Commerce.Web\Web.Azure.config

REM Copies the XDT transforms into ArtOfGroundFighting.Initializer root
copy %1web.Debug.config %1\..\ArtOfGroundFighting.Initializer\App.Debug.config
copy %1web.AzureSandbox.config %1\..\ArtOfGroundFighting.Initializer\App.AzureSandbox.config
copy %1web.Azure.config %1\..\ArtOfGroundFighting.Initializer\App.Azure.config

REM Copies the Email Templates into the  
xcopy %1\EmailTemplates\Customer %1\..\Commerce.Web\EmailTemplates\Customer /Y /S
xcopy %1\EmailTemplates\Shared %1\..\Commerce.Web\EmailTemplates\Customer /Y /S
