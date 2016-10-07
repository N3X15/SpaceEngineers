rm -rfv build dist
pyinstaller main.spec
rm ../prep.exe
copy dist\prep.exe ..\
pause