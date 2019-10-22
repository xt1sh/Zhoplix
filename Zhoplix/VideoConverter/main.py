import sys 
sys.path.append('C:\Code\ASP.NET\Zhoplix\Zhoplix\bin\Debug\netcoreapp3.0\Lib')
from converter import Converter
import math

def create_thumbnails(path):
    c = Converter('C:/Program Files/ffmpeg/bin/ffmpeg.exe', 'C:/Program Files/ffmpeg/bin/ffprobe.exe')
    info = c.probe(path)
    thumbnail_duration = math.ceil(math.log10(info.format.duration))
    count = int(info.format.duration / thumbnail_duration)
    directory = '/'.join(path.split('/')[0:-1])
    for i in range(0, count):
        c.thumbnail(path, i * thumbnail_duration, '{0}/Thumbnails/{1}.png'.format(directory, i))

def convert_for_thumbnails(path, newPath):
    c = Converter('C:/Program Files/ffmpeg/bin/ffmpeg.exe', 'C:/Program Files/ffmpeg/bin/ffprobe.exe')
    options = {
        'format': 'mp4',
        'video': {
            'codec': 'h264',
            'width': 128,
            'height': 72,
        }
    }
    conv = c.convert(path, newPath, options, timeout=None)
    for timecode in conv:
        pass

if __name__ == '__main__':
    print('zhopa')
    convert_for_thumbnails('C:/Code/ASP.NET/Zhoplix/Zhoplix/wwwroot/Videos/Uploaded/0.mp4', 'C:/Code/ASP.NET/Zhoplix/Zhoplix/wwwroot/Videos/Uploaded/0thumb.mp4')
    create_thumbnails('C:/Code/ASP.NET/Zhoplix/Zhoplix/wwwroot/Videos/Uploaded/0thumb.mp4')