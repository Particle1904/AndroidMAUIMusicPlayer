using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Lib.src.Interfaces
{
    public interface IFileManipulatorService
    {
        public Task GetSoundFiles();
    }
}
