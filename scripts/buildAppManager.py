import PyInstaller.__main__
import os
    
PyInstaller.__main__.run([  
     '--onefile',
     '--windowed',
     os.path.join('./', 'main.py'),                        
])