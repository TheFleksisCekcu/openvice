﻿//////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// This program is free software: you can redistribute it and/or modify it under the terms of the 
/// GNU General Public License as published by the Free Software Foundation, either version 3 of the License, 
/// or (at your option) any later version.
/// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the 
/// implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
/// for more details.
/// You should have received a copy of the GNU General Public License along with this program. If not, see
/// http://www.gnu.org/licenses/.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace OpenVice.VM
{
    /// <summary>
    ///  * Interface for a collection of functions that can be exported to a game script
    ///  * interface.
    ///  *
    ///  * For example a collection of functions that control the time of day, or create
    ///  * objects would
    ///  * be the collected within one ScriptModule with a sensible name like
    ///  * "Environment" or "Objects"
    ///  
    ///  * Интерфейс для набора функций, которые можно экспортировать в скрипт игры
    ///  * интерфейс.
    ///  *
    ///  * Например, набор функций, которые контролируют время суток или создают
    ///  * объекты будут быть собранным в одном ScriptModule с разумным именем, 
    ///  * например «Окружающая среда» или «Объекты»
    /// </summary>
    public class ScriptModule
    {
        private readonly string name;
        private SortedDictionary<uint, ScriptFunctionMeta> functions = new SortedDictionary<uint, ScriptFunctionMeta>();

        public ScriptModule(string Name)
	    {
		    name = Name;
	    }

        /// <summary>
        /// The name of this ScriptModule instance.
        /// </summary>
        /// <returns>A string with the name.</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Binds an opcode to a function.
        /// </summary>
        /// <typeparam name="Tfunc">A function delegate to bind.</typeparam>
        /// <param name="ID"></param>
        /// <param name="Argc"></param>
        /// <param name="Function"></param>
        public void Bind<Tfunc>(ushort ID, int Argc, Tfunc Function)
        {
            Type T = typeof(Tfunc);

            ScriptFunctionMeta MetaFunction = new ScriptFunctionMeta();
            if(T == typeof(ScriptFunction))
                MetaFunction.Function = (ScriptFunction)Convert.ChangeType(Function, typeof(ScriptFunction));
            else
                MetaFunction.BooleanFunction = (ScriptFunctionBoolean)Convert.ChangeType(Function, typeof(ScriptFunctionBoolean));
            MetaFunction.Arguments = Argc;

            functions.Add(ID, MetaFunction);
            /*functions.insert(

            {
                id,
			    {

                    [=] (ScriptArguments& args) { script_bind.do_unpacked_call(function, args); },
				    argc, "opcode", ""
			    }
		    });*/
	    }

	    public void ReserveFunctions(uint Nr)
        {
            //Resizes the unordered_map: http://www.cplusplus.com/reference/unordered_map/unordered_map/reserve/
            //This shouldn't be neccessary in C#.
            //m_Functions.reserve(nr);
        }

        /// <summary>
        /// Finds the function based on an opcode (ID).
        /// </summary>
        /// <param name="ID">The opcode (ID) of a function.</param>
        /// <param name="Func">The function to find.</param>
        /// <returns></returns>
        public bool FindOpcode(ushort ID, out ScriptFunctionMeta Func)
        {
            try
            {
                var it = functions[ID];
            }
            catch (KeyNotFoundException)
            {
                Func = new ScriptFunctionMeta();
                return false;
            }

            Func = functions[ID];
            return true;
        }
    }
}
