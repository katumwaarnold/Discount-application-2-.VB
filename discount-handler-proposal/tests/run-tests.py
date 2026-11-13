#!/usr/bin/env python3
"""Compile and run the exact proposed handler using simulated controls (requires .NET SDK 8)."""
from pathlib import Path
import os
import shutil
import subprocess
import sys
import tempfile

here = Path(__file__).resolve().parent
handler = (here.parent / 'corrected-handler.vb').read_text(encoding='utf-8')
template = (here / 'Program.template').read_text(encoding='utf-8')
if template.count('__HANDLER__') != 1:
    sys.exit('Expected exactly one handler placeholder')
with tempfile.TemporaryDirectory(prefix='discount-vb-test-') as temp:
    folder = Path(temp)
    shutil.copy2(here / 'Test.vbproj', folder / 'Test.vbproj')
    (folder / 'Program.vb').write_text(template.replace('__HANDLER__', handler.rstrip('\n')), encoding='utf-8')
    dotnet = os.environ.get('DOTNET', 'dotnet')
    try:
        result = subprocess.run([dotnet, 'run', '--project', str(folder / 'Test.vbproj')], check=False)
    except FileNotFoundError:
        sys.exit('Install .NET SDK 8 or set DOTNET to the dotnet executable')
    sys.exit(result.returncode)
