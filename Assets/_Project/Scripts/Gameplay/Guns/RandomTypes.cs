using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomTypes
{
    private List<Type> types;

    public RandomTypes(params Type[] types) {
        this.types = types.ToList();
    }

    public Type GetRandomType() {
        return RandomType();
    }
    
    public Type GetRandomType(params Type[] dontIncludeTypes) {
        Type finallyType = null;
        
        if (!ArrayInitialized(dontIncludeTypes)) 
            return RandomType();

        while (finallyType == null) {
            var exemplaryType = RandomType();

            if (!dontIncludeTypes.Contains(exemplaryType))
                finallyType = exemplaryType;
        }
        
        return finallyType;
    }
    
    public Type GetRandomType(List<Type> dontIncludeTypes) {
        Type finallyType = null;
        
        if (!ArrayInitialized(dontIncludeTypes)) 
            return RandomType();

        while (finallyType == null) {
            var exemplaryType = RandomType();

            if (!dontIncludeTypes.Contains(exemplaryType))
                finallyType = exemplaryType;
        }
        
        return finallyType;
    }
    
    public List<Type> GetRandomDifferentTypes(int count) {
        if (count >= types.Count) throw new Exception("Too less type to find that count of types");
        var typesList = new List<Type>();

        while (typesList.Count < count) {
            var exemplaryType = RandomType();

            if(!typesList.Contains(exemplaryType))
                typesList.Add(exemplaryType);
        }
        
        return typesList;
    }
    
    public List<Type> GetRandomDifferentTypes(int count, params Type[] dontIncludeTypes) {
        if (count >= types.Count) throw new Exception("Too less type to find that count of types");
        var typesList = new List<Type>();

        if (!ArrayInitialized(dontIncludeTypes)) {
            typesList.Add(RandomType());
            return typesList;
        }

        while (typesList.Count < count) {
            var exemplaryType = RandomType();
            
            if(!dontIncludeTypes.Contains(exemplaryType))
                if(!typesList.Contains(exemplaryType))
                    typesList.Add(exemplaryType);
        }
        
        return typesList;
    }

    private bool ArrayInitialized(Type[] arrayToCheck) {
        if (arrayToCheck == null || arrayToCheck.Length <= 0)
            return false;

        return true;
    }
    private bool ArrayInitialized(List<Type> listToCheck) {
        return ArrayInitialized(listToCheck.ToArray());
    }
    private Type RandomType() {
        int randomIndex = Random.Range(0, types.Count);
        return types[randomIndex];
    }
}
