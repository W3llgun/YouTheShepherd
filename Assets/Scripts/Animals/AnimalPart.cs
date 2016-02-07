using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalPart : MonoBehaviour {

	public static Dictionary<string, string> legendaryList = new Dictionary<string, string> () {
		{"CorpsElephantAileOiseau","Dragon"},
		{"CorpsChevalTeterhinoceros","Licorne"}
	};

	public string description; 

	public static string compareTo (List<string> all) {
		foreach (string S1 in all) {
			foreach(string S2 in all) {

				string combinaison = S1+S2;
				if(legendaryList.ContainsKey(combinaison))
                {
                    if(legendaryList[combinaison].Equals("Dragon"))
                    {
                        ObjectifsManager.Instance.dragon = true;
                    }
                    else
                    {
                        ObjectifsManager.Instance.licorne = true;
                    }
                    return legendaryList[combinaison];
                }
					
			}
		}

		return "";
	}
}
