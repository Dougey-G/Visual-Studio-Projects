using System;
using System.Collections.Generic;
using System.Reflection;
using GDD3400_RTS_Lib;

namespace GDD3400_Pathfinding
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Load DLLS - loads the DLLs that represent other agents
        /// </summary>
        /// <param name="agents">collection of agents created by
        /// this method to be sent to the GameManager</param>
        static void LoadDLLs(List<Agent> agents)
        {
            // TODO: set this string to indicate the names of the DLLs
            // You MUST use the absolute path (from C:), you cannot use
            // relative paths for this.  Put the other DLLs wherever you
            // want, open their folder in Windows Explorer, copy the
            // file location from the address bar by clicking inside
            // the bar and then add the name of the individual dll.
            // If you are just developing, put 4 copies of your own
            // DLL here - found in the Debug folder of the
            // GDD3400_Pathfinding project folder within this solution.
            string[] dlls =
            {
                @"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\GDD3400_PlanningAgent_Lib.dll",
                @"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll",
                @"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll",
                @"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\GDD3400_PlanningAgent_Lib.dll"
                //@"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll",
                //@"F:\Programming\Visual Studio Projects\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll"
                //@"D:\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\GDD3400_PlanningAgent_Lib.dll",
                //@"D:\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll",
                //@"D:\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll",
                //@"D:\Visual-Studio-Projects\DanasRtsGame\GDD3400_Pathfinding\GDD3400_Pathfinding\bin\x86\Debug\BasePlanningAgent.dll"
            };

            // For each of the DLL files, load it and then launch
            // an object of the type in the DLL
            for (int i = 0; i < dlls.Length; ++i)
            {
                var DLL = Assembly.LoadFile(dlls[i]);

                foreach (Type type in DLL.GetExportedTypes())
                {
                    agents.Add((Agent)Activator.CreateInstance(type));
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            List<Agent> agents = new List<Agent>();
            LoadDLLs(agents);

            using (GameManager game = new GameManager(agents))
            {
                game.IsMouseVisible = true;
                game.Run();
            }
        }
    }
#endif
}

