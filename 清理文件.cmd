for /d /r . %%d in (bin obj AppPackages BundleArtifacts .vs) do @if exist "%%d" rd /s/q "%%d"