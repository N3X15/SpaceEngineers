# -*- mode: python -*-
import sys
a = Analysis([os.path.join('prep.py')],
             hiddenimports=[],
             hookspath=None)

pyz = PYZ(a.pure)

filename = 'prep'
ename=os.path.join('build-dist', filename)
if sys.platform.startswith('win') or sys.platform.startswith('microsoft'):
	ename += '.exe'

exe = EXE(pyz,
          a.scripts,
          a.binaries,
          a.zipfiles,
          a.datas,
          name=ename,
          debug=False,
          strip=None,
          console=True)
app = BUNDLE(exe, name=filename+'.app')
