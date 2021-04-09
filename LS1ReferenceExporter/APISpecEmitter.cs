//#define MINIMUM_BUILD
using libLS1APISpec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace LS1ReferenceExporter
{
    public class APISpecEmitter
    {
        public bool HideRestricted;

        public APISpecEmitter(LS1APISpec apispec)
        {
            APISpec = apispec;
        }
        public LS1APISpec APISpec { get; private set; }

        void EmitLine(string line)
        {
            Output += line + Environment.NewLine;
        }

        void Emit(LS1Event e)
        {
            if (HideRestricted && e.Restricted)
                return;

            EmitLine(string.Format("## Event: {0}", e.Name));

            if (e.Restricted)
                EmitLine("- Restricted: Yes");

#if MINIMUM_BUILD
            if (e.MinimumBuild>0)
                EmitLine(string.Format("- Minimum Build: {0}", e.MinimumBuild));
#endif

            if (!string.IsNullOrEmpty(e.Description))
                EmitLine(string.Format("{0}", e.Description));

            EmitLine("");
        }

        string EmitDeclarations(ObservableCollection<LS1Parameter> parameters, string separator = ", ", bool encloseRequired = true)
        {
            string output = string.Empty;

            foreach (LS1Parameter p in parameters)
            {
                if (output.Length > 0)
                    output += " ";
                output += EmitDeclaration(p, separator, encloseRequired);
            }

            return output;
        }

        string EmitDeclaration(LS1Parameter parameter, string separator = ", ", bool encloseRequired = true)
        {
            string formatOptional = "<{0}>";
            string formatRequired = "[{0}]";

            if (encloseRequired)
            {
                formatRequired = "<{0}>";
                formatOptional = "[{0}]";
            }


            string output = string.Empty;
            if (!string.IsNullOrEmpty(parameter.Constant))
            {
                output = string.Format("\"{0}\"", parameter.Constant);

                if (parameter.FollowingParameters != null && parameter.FollowingParameters.Count > 0)
                {
                    output += separator + EmitDeclarations(parameter.FollowingParameters, separator, encloseRequired);
                }

                if (parameter.Optional)
                {
                    output = string.Format(formatOptional, output);
                }

                return output;
            }

            if (parameter.Enum != null && parameter.Enum.Count > 0)
            {
                output = string.Empty; // string.Format("\"{0}\"", parameter.Constant);

                foreach (string s in parameter.Enum)
                {
                    if (output.Length > 0)
                        output += "|";
                    output += string.Format("\"{0}\"", s);
                }

                if (parameter.FollowingParameters != null && parameter.FollowingParameters.Count > 0)
                {
                    output += separator + EmitDeclarations(parameter.FollowingParameters, separator, encloseRequired);
                }

                if (parameter.Optional)
                {
                    output = string.Format(formatOptional, output);
                }

                return output;
            }

            if (parameter.Greedy)
            {
                output += "...";
            }


            if (!string.IsNullOrEmpty(parameter.Type))
            {
                if (output.Length > 0)
                    output += " ";
                output += string.Format("[{0}](#type-{0})", parameter.Type);
            }

            if (!string.IsNullOrEmpty(parameter.Name))
            {
                if (output.Length > 0)
                    output += " ";
                output += "`" + parameter.Name + "`";
            }

            if (parameter.Default != null)
            {
                output += "=" + parameter.Default.ToString(Newtonsoft.Json.Formatting.None);
            }


            if (parameter.FollowingParameters != null && parameter.FollowingParameters.Count > 0)
            {
                output += separator + EmitDeclarations(parameter.FollowingParameters, separator, encloseRequired);
            }

            if (parameter.ParameterGroup != null && parameter.ParameterGroup.Count > 0)
            {
                string pgoutput = string.Empty;

                foreach (LS1Parameter p in parameter.ParameterGroup)
                {
                    if (pgoutput.Length > 0)
                        pgoutput += "|";
                    pgoutput += EmitDeclaration(p, separator, encloseRequired);
                }

                output += " [" + pgoutput + "]";
            }


            if (parameter.Optional)
            {
                output = string.Format(formatOptional, output);
            }
            else
            {
                if (encloseRequired)
                    output = string.Format(formatRequired, output);
            }

            return output;
        }

        void Emit(LS1Command command, LS1CommandForm form)
        {
            if (form.Parameters== null || form.Parameters.Count == 0)
            {
                EmitLine(string.Format("* `{0}`",command.Name));
            }
            else
            {
                string parameters = string.Empty;

                foreach(LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " ";
                    parameters += EmitDeclaration(p, " ");
                }

                EmitLine(string.Format("* `{0}` {1}", command.Name, parameters));
            }

#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  * Minimum Build: {0}", form.MinimumBuild));
#endif
            if (!string.IsNullOrEmpty(form.Description))
                EmitLine("  * "+form.Description);           
        }

        void Emit(LS1Command command)
        {
            if (HideRestricted && command.Restricted)
                return;

            EmitLine(string.Format("## Command: {0}", command.Name));

            if (command.Restricted)
            {
                EmitLine("- Restricted: Yes");
                EmitLine("");
            }

            if (command.Forms!=null)
            {
                EmitLine("### Syntax");
                foreach(LS1CommandForm f in command.Forms)
                {
                    Emit(command, f);
                }
            }
        
            EmitLine("");
        }

        void Emit(LS1TopLevelObject tlo, LS1TopLevelObjectForm form)
        {
            string useDescription = string.Empty;
            if (!string.IsNullOrEmpty(form.Description))
            {
                useDescription = ": " + form.Description;
            }

            if (form.Parameters == null || form.Parameters.Count == 0)
            {                
                EmitLine(string.Format("- [{1}](#type-{1}) `{0}`{2}",tlo.Name,form.Type,useDescription));
            }
            else
            {
                string parameters = string.Empty;

                foreach (LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " `,` ";
                    parameters += EmitDeclaration(p, " `,` ", false);
                }

                EmitLine(string.Format("- [{2}](#type-{2}) `{0}[` {1} `]`{3}", tlo.Name, parameters, form.Type,useDescription));
            }

#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  - Minimum Build: {0}", form.MinimumBuild));
#endif
//            if (!string.IsNullOrEmpty(form.Description))
//                EmitLine(" - " + form.Description);
        }

        void Emit(LS1TopLevelObject tlo)
        {
            if (HideRestricted && tlo.Restricted)
                return;

            EmitLine(string.Format("## TLO: {0}", tlo.Name));

            if (tlo.Restricted)
                EmitLine("Restricted: Yes");

            if (tlo.Forms != null)
            {
                foreach (LS1TopLevelObjectForm f in tlo.Forms)
                {
                    Emit(tlo, f);
                }
            }

            EmitLine("");
        }

        void Emit(LS1Type t, LS1TypeInitializerForm form)
        {
            string useDescription = string.Empty;
            if (!string.IsNullOrEmpty(form.Description))
            {
                useDescription = ": " + form.Description;
            }

            if (form.Parameters == null || form.Parameters.Count == 0)
            {
                EmitLine(string.Format("`{0}`{1}",t.Name,useDescription));
            }
            else
            {
                string parameters = string.Empty;

                foreach (LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " `,` ";
                    parameters += EmitDeclaration(p, " `,` ", false);
                }

                EmitLine(string.Format("- `{0}[` {1} `]`{2}", t.Name, parameters,useDescription));
            }

#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  - Minimum Build: {0}", form.MinimumBuild));
#endif
        }

        void Emit(LS1Type t, LS1TypeInitializer ti)
        {
            if (ti.Forms!=null && ti.Forms.Count>0)
            {
                EmitLine("### Initializers");
                foreach (LS1TypeInitializerForm tif in ti.Forms)
                {
                    Emit(t, tif);
                }
            }
        }

        void Emit(LS1Type t, LS1TypeAsString tas)
        {
           if (!string.IsNullOrEmpty(tas.Constant))
            {
                EmitLine(string.Format("As Text: \"{0}\"", tas.Constant));
                return;
            }

           if (!string.IsNullOrEmpty(tas.Member))
            {
                EmitLine(string.Format("As Text: Same as `{0}`", tas.Member));
            }

            if (!string.IsNullOrEmpty(tas.Description))
            {
                EmitLine(string.Format("As Text: {0}", tas.Description));
            }
        }

        void Emit(LS1Type t, LS1TypeIndexForm form)
        {
            string useDescription = string.Empty;
            if (!string.IsNullOrEmpty(form.Description))
            {
                useDescription = ": " + form.Description;
            }

            string useType = "???";
            if (!string.IsNullOrEmpty(form.Type))
            {
                useType = string.Format("[{0}](#type-{0})", form.Type);
            }


            if (form.Parameters == null || form.Parameters.Count == 0)
            {
                EmitLine(string.Format("- {2} `{0}`{1}",t.Name,useDescription,useType));
            }
            else
            {
                string parameters = string.Empty;

                foreach (LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " `,` ";
                    parameters += EmitDeclaration(p, " `,` ", false);
                }

                EmitLine(string.Format("- {3} `{0}[` {1} `]`{2}", t.Name, parameters,useDescription,useType));
            }

#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  - Minimum Build: {0}", form.MinimumBuild));
#endif
        }

        void Emit(LS1Type t, LS1TypeIndex ti)
        {
            if (ti.Forms != null && ti.Forms.Count > 0)
            {
                EmitLine("### Indexers");
                foreach (LS1TypeIndexForm tif in ti.Forms)
                {
                    Emit(t, tif);
                }
            }
        }

        void Emit(LS1Type t, LS1TypeMember tm, LS1TypeMemberForm form)
        {
            string useDescription = string.Empty;
            if (!string.IsNullOrEmpty(form.Description))
            {
                useDescription = ": " + form.Description;
            }

            string useType = "???";
            if (!string.IsNullOrEmpty(form.Type))
            {
                useType = string.Format("[{0}](#type-{0})",form.Type);
            }

            if (form.Parameters == null || form.Parameters.Count == 0)
            {
                EmitLine(string.Format("- {2} `{1}`{3}",t.Name,tm.Name, useType, useDescription));
            }
            else
            {
                string parameters = string.Empty;

                foreach (LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " `,` ";
                    parameters += EmitDeclaration(p, " `,` ", false);
                }

                EmitLine(string.Format("- {2} `{1}[` {3} `]`{4}", t.Name, tm.Name,useType, parameters, useDescription));
            }

            if (tm.Restricted)
                EmitLine("  - Restricted: Yes");
#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  - Minimum Build: {0}", form.MinimumBuild));
#endif
        }

        void Emit(LS1Type t, LS1TypeMember tm)
        {
            if (HideRestricted && tm.Restricted)
                return;

            if (tm.Forms != null && tm.Forms.Count > 0)
            {
                foreach (LS1TypeMemberForm tif in tm.Forms)
                {
                    Emit(t, tm, tif);
                }
            }
            else
            {
                EmitLine(string.Format("- ??? `{1}[`???`]`",t.Name,tm.Name));
                if (tm.Restricted)
                    EmitLine("  - Restricted: Yes");
            }
        }

        void Emit(LS1Type t, LS1TypeMethod tm, LS1TypeMethodForm form)
        {
            string useDescription = string.Empty;
            if (!string.IsNullOrEmpty(form.Description))
            {
                useDescription = ": " + form.Description;
            }

            if (form.Parameters == null || form.Parameters.Count == 0)
            {
                EmitLine(string.Format("- `{1}`{2}", t.Name, tm.Name, useDescription));
            }
            else
            {
                string parameters = string.Empty;

                foreach (LS1Parameter p in form.Parameters)
                {
                    if (parameters.Length > 0)
                        parameters += " `,` ";
                    parameters += EmitDeclaration(p, " `,` ", false);
                }

                EmitLine(string.Format("- `{1}[` {2} `]`{3}", t.Name, tm.Name, parameters, useDescription));
            }

            if (tm.Restricted)
                EmitLine("  - Restricted: Yes");
#if MINIMUM_BUILD
            if (form.MinimumBuild > 0)
                EmitLine(string.Format("  - Minimum Build: {0}", form.MinimumBuild));
#endif
        }


        void Emit(LS1Type t, LS1TypeMethod tm)
        {
            if (HideRestricted && tm.Restricted)
                return;

            if (tm.Forms != null && tm.Forms.Count > 0)
            {
                foreach (LS1TypeMethodForm tif in tm.Forms)
                {
                    Emit(t, tm, tif);
                }
            }
            else
            {
                EmitLine(string.Format("- `{1}[`???`]`", t.Name, tm.Name));
                if (tm.Restricted)
                    EmitLine("  - Restricted: Yes");
            }
        }

        void Emit(LS1Type t)
        {
            if (HideRestricted && t.Restricted)
                return;

            EmitLine(string.Format("## Type: {0}", t.Name));

            if (!string.IsNullOrEmpty(t.BaseType))
            {
                EmitLine(string.Format("- Base Type: [{0}](#type-{0})",t.BaseType));
            }

            if (!t.Persistent)
            {
                EmitLine("- Persistent: No ([weakref](#type-weakref) not supported)");
            }

            if (t.Restricted)
                EmitLine("- Restricted: Yes");

#if MINIMUM_BUILD
            if (t.MinimumBuild > 0)
                EmitLine(string.Format("- Minimum Build: {0}", t.MinimumBuild));
#endif

            if (!string.IsNullOrEmpty(t.Description))
                EmitLine(string.Format("{0}", t.Description));

            EmitLine("");

            if (t.Initializer!=null)
            {
                Emit(t, t.Initializer);
                EmitLine("");
            }

            if (t.Index!=null)
            {
                Emit(t, t.Index);
                EmitLine("");
            }

            if (t.AsString!=null)
            {
                Emit(t, t.AsString);
                EmitLine("");
            }

            EmitLine("### Members");
            if (t.Members!=null && t.Members.Count > 0)
            {
                foreach(var tm in t.Members)
                {
                    Emit(t, tm);
                }
                EmitLine("");
            }
            else
            {
                EmitLine("none.");
            }

            EmitLine("### Methods");
            if (t.Methods != null && t.Methods.Count > 0)
            {
                foreach (var tm in t.Methods)
                {
                    Emit(t, tm);
                }
                EmitLine("");
            }
            else
            {
                EmitLine("none.");
            }

            EmitLine("");
            EmitLine("");
        }

        public void Go()
        {
            Output = string.Empty;
            if (APISpec == null)
                return;

            EmitLine(string.Format("{0} API Specification (LavishScript)", APISpec.Name));
            EmitLine(APISpec.Description);
            EmitLine("---");
            EmitLine("# Events");

            if (APISpec.Events==null || APISpec.Events.Count==0)
            {
                EmitLine("none.");
            }
            else
            {
                foreach(LS1Event e in APISpec.Events)
                {
                    Emit(e);
                }
            }

            EmitLine("");
            EmitLine("---");
            EmitLine("# Commands");

            if (APISpec.Commands == null || APISpec.Commands.Count == 0)
            {
                EmitLine("none.");
            }
            else
            {
                foreach (LS1Command e in APISpec.Commands)
                {
                    Emit(e);
                }
            }
            EmitLine("");
            EmitLine("---");
            EmitLine("# Top-Level Objects");

            if (APISpec.TopLevelObjects == null || APISpec.TopLevelObjects.Count == 0)
            {
                EmitLine("none.");
            }
            else
            {
                foreach (LS1TopLevelObject e in APISpec.TopLevelObjects)
                {
                    Emit(e);
                }
            }

            EmitLine("");
            EmitLine("---");
            EmitLine("# Types");

            if (APISpec.Types == null || APISpec.Types.Count == 0)
            {
                EmitLine("none.");
            }
            else
            {
                foreach (LS1Type e in APISpec.Types)
                {
                    Emit(e);
                }
            }



        }

        public string Output { get; private set; }
    }
}
