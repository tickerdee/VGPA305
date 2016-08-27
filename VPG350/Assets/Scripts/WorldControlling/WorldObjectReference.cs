using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldObjectReference{

	//To use this
	//IMPORTANT only add objects that will NOT: get destroyed, change refernce i.e. set = to something (This changes the pointer linkage)
		/*
		 * ---To add an object---
		 * Lets say you have an object of type MyObjectType and its name is MyObject i.e. MyObjectType MyObject;
		 * You would write: WorldObjectReferencer.GetInstance().AddObject<MyObjectType>(MyOjbect);
		 * 
		 * ---To get an object---
		 * Lets say you KNOW an object of type MyObjectType should have already been or will be added.
		 * You would write: MyObjectType foundObject = WorldObjectReferencer.GetInstance().GetObject<MyObjectType>();
		 */

	//Singleton pattern this bitch
	static WorldObjectReference instance;

	public static WorldObjectReference GetInstance(){

		if(instance == null){
			instance = new WorldObjectReference();
		}

		return instance;
	}

	//List of c# base class references
	List<System.Object> Objects;

	//If the list doesn't currently exist make it
	//Add object of Generic type T
	public void AddObject<T>(T obj){

		if(Objects == null){
			Objects = new List<System.Object>();
		}

		Objects.Add((System.Object)obj);
	}

	//Take in a generic type. loop through the list looking for a reference that matches that type
		//Possibly cleanup old references (nulled) here
	public T GetObject<T>(){

		if(Objects == null)
			return default(T);

		foreach(System.Object tempObj in Objects){

			if(tempObj != null){
				if(tempObj is T){
					return (T)tempObj;
				}
			}else{
				Objects.Remove(tempObj);
			}
		}//End Foreach

		return default(T);
	}

	public void Destroy(){

		Debug.Log("World Object referencer cleaned up");

		while(Objects.Count > 0){
			Objects.RemoveAt(Objects.Count - 1);
		}

		instance = null;
	}
}//End Class