REM %1 => $(ProjectDir)
REM %2 => $(SolutionDir)
REM %3 => $(TargetDir)

REM Copies library contents into binary output
xcopy %2\lib %3 /Y /S

