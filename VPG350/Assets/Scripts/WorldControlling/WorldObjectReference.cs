using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldObjectReference{

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
}
