using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaOrganiserCore
{
    public interface Imp3Manager
    {
		List<string> GetAllMp3s(string directory, bool includeSubDirs);
		string CleanMp3(string filePath);
    }
}
