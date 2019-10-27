@echo off
for /f "delims=" %%A in (conf.vars) do set %%A
for /f "delims=" %%A in (conf.vars.local) do set %%A
@echo on