namespace RTS.Engine.Renderer;

class InvalidRidTypeException<T>() : Exception($"RID must point to type {typeof(T).FullName}") where T: IResource;
