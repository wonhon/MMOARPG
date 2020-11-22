using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime.UI
{
    public class TextT : Text
    {
    	private static MethodInfo replaceMethod = null;

    	public override string text
    	{
    		get{
    			string text = base.text;
    			if(replaceMethod == null)
    			{
    				var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
		            foreach (var assembly in assemblies)
		            {
                        if(!assembly.GetName().Name.Contains("Assembly-CSharp"))
                        {
                            continue;
                        }

		                var types = assembly.GetTypes();
		                foreach (var each in types)
		                {
			                if( each.ToString() == "UIStaticFunction")
			                {
								var func = each.GetMethod("ReplaceStrByID", 
						                    System.Reflection.BindingFlags.IgnoreCase 
						                    | System.Reflection.BindingFlags.NonPublic
						                    | System.Reflection.BindingFlags.Static);
						        if(func != null)
						        {
						        	replaceMethod = func;
						        	break;
						        }
			                }
				    	}
				    }
    			}
    			if(replaceMethod != null)
    			{
    				return (string)replaceMethod.Invoke(null,new object[]{text});
    			}
    			else
    			{
    				return text;
    			}
    		}
    		set
    		{
    			base.text = value;
    		}
    	}


    }
}