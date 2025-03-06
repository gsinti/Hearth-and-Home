using System;
using System.Reflection;
using Server;
using Server.Commands;

namespace Server.Commands
{
    public class ReloadScripts
    {
        public static void Initialize()
        {
            CommandSystem.Register("ReloadScripts", AccessLevel.Administrator, new CommandEventHandler(ReloadScripts_OnCommand));
        }

        public static void ReloadScripts_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Recompiling scripts...");

            try
            {
                // Store reference to old script assembly
                Assembly oldAssembly = ScriptCompiler.Assemblies.Length > 0 ? ScriptCompiler.Assemblies[0] : null;

                // Trigger script recompilation
                bool success = ScriptCompiler.Compile(false, false);

                if (success)
                {
                    e.Mobile.SendMessage("Scripts recompiled successfully!");

                    // Check if the new assembly is different from the old one
                    Assembly newAssembly = ScriptCompiler.Assemblies.Length > 0 ? ScriptCompiler.Assemblies[0] : null;
                    if (newAssembly != null && newAssembly != oldAssembly)
                    {
                        e.Mobile.SendMessage("New script assembly loaded.");
                    }
                }
                else
                {
                    e.Mobile.SendMessage("Script recompilation failed! Check console for errors.");
                }
            }
            catch (Exception ex)
            {
                e.Mobile.SendMessage("Error during script recompilation: " + ex.Message);
            }
        }
    }
}
